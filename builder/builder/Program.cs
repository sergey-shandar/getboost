using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using builder.MarkDown;
using Framework.G1;

namespace builder
{
    partial class Program
    {
        private static IEnumerable<SrcPackage> CreatePackageList(
            string name,
            string path,
            IEnumerable<SrcPackage> packageListConfig)
        {
            var firstPackage = packageListConfig.First();
            var dir = new Dir(new DirectoryInfo(path), "");
            var remainder = dir.FileList().ToHashSet();
            foreach (var p in packageListConfig.Skip(1))
            {
                var fileList = dir.FileList(p.FileList);
                remainder.ExceptWith(fileList);
                yield return new SrcPackage(
                    name:
                        name +
                        "_" +
                        p.Name.Select(n => n, () => string.Empty),
                    package: p,
                    fileList: fileList
                );
            }
            //
            remainder.UnionWith(dir.FileList(firstPackage.FileList));
            yield return new SrcPackage(
                name: name,
                package: firstPackage,
                fileList: remainder
            );
        }

        private static IEnumerable<string> MakeSrcLibrary(
            Library libraryConfig, string src)
        {
            var name = libraryConfig.Name;
            var id = "boost_" + name;
            return new Library(
                id,
                src,
                CreatePackageList(
                    name, src, libraryConfig.PackageList
                ).ToOptionalClass()
            ).
            Create();
        }

        private static void ScanCompiledFileSet(
            Dictionary<string, Dictionary<string, CompiledPackage>> compilerDictionary,
            Dictionary<string, CompiledLibrary> libraryDictionary)
        {
            foreach (var dir in new DirectoryInfo(Config.BoostDir).GetDirectories("stage/msvc-14.2/*"))
            {
                var libDir = dir.CreateSubdirectory("lib");
                foreach (var file in libDir.GetFiles("*boost*"))
                {
                    var split = file.Name.SplitFirst('-');
                    var library = split.Before.SplitFirst('_').After;
                    var compiler = split.After.SplitFirst('-').Before;

                    //
                    var compiledLibrary = libraryDictionary.GetOrAddNew(library);
                    var compiledPackage =
                        compiledLibrary.PackageDictionary.GetOrAddNew(compiler);
                    compiledPackage.AddFile(libDir.FullName, file.Name);

                    // add the compiler and add the library to the compiler.
                    compilerDictionary.GetOrAddNew(compiler)[library] =
                        compiledPackage;
                }
            }
        }

        private static A A(string name, string library, Version version)
            => T.A(
                name,
                "http://nuget.org/packages/" +
                    library +
                    "/" +
                    version);

        private static A A(string url, Version version)
            => A(url, url, version);

        private static void CreateBinaryNuspec(
            string id,
            string compiler,
            IEnumerable<Targets.ItemDefinitionGroup> itemDefinitionGroupList,
            IEnumerable<Nuspec.File> fileList,
            IEnumerable<Nuspec.Dependency> dependencyList,
            Optional<string> name)
        {
            var info = Config.CompilerMap[compiler];
            var description =
                id +
                ". Compiler: " +
                info.Name +
                ".";
            Nuspec.Create(
                id,
                id,
                SrcPackage.CompilerVersion(info),
                description,
                itemDefinitionGroupList,
                fileList,
                Enumerable.Empty<CompilationUnit>(),
                dependencyList,
                new[] { "binaries", compiler }.
                    Concat(name.ToEnumerable()));
        }

        static void Main(string[] args)
        {
            var doc = new Doc();

            // Changes
            {
                doc = doc[T.H1("Release Notes")];
                foreach (var change in Config.Release)
                {
                    doc = doc[change];
                }
            }

            // headers only library.
            {
                doc = doc
                    [T.H1("Headers Only Libraries")]
                    [T.List[A("boost", Config.Version)]];
                var path = Path.Combine(Config.BoostDir, "boost");
                var fileList =
                    new Dir(new DirectoryInfo(path), "boost").
                    FileList(f => true);
                Nuspec.Create(
                    "boost",
                    "boost",
                    Config.Version,
                    "boost",
                    new[]
                    {
                        new Targets.ItemDefinitionGroup(
                            clCompile:
                                new Targets.ClCompile(
                                    additionalIncludeDirectories:
                                        new[]
                                        {
                                            Targets.PathFromThis(
                                                Targets.IncludePath)
                                        }
                                )
                        )
                    },
                    fileList.Select(
                        f =>
                            new Nuspec.File(
                                Path.Combine(Config.BoostDir, f),
                                Path.Combine(Targets.IncludePath, f)
                            )
                    ),
                    new CompilationUnit[0],
                    new Nuspec.Dependency[0],
                    new[] { "headers" }
                );
            }

            // source libraries.
            doc = doc[T.H1("Source Libraries")];
            var srcLibList = new List<string>();
            foreach (var directory in Directory
                .GetDirectories(Path.Combine(Config.BoostDir, "libs")))
            {
                var src = Path.Combine(directory, "src");
                if (Directory.Exists(src))
                {
                    var name = Path.GetFileName(directory);

                    var libraryConfig = Config
                        .LibraryList
                        .Where(l => l.Name == name)
                        .FirstOrDefault()
                        ?? new Library(name);

                    foreach (var libName in MakeSrcLibrary(libraryConfig, src))
                    {
                        var fullName = "boost_" + libName + "-src";
                        srcLibList.Add(libName);
                        doc = doc[T.List[A(
                            libName, fullName, Config.Version)]];
                    }
                }
            }
            Nuspec.Create(
                    "boost-src",
                    "boost-src",
                    Config.Version,
                    "boost-src",
                    Enumerable.Empty<Targets.ItemDefinitionGroup>(),
                    Enumerable.Empty<Nuspec.File>(),
                    Enumerable.Empty<CompilationUnit>(),
                    srcLibList.Select(srcLib => new Nuspec.Dependency(
                        "boost_" + srcLib + "-src", Config.Version.ToString())),
                    new[] { "sources" });

            // create dictionaries for binary NuGet packages.
            doc = doc[T.H1("Precompiled Libraries")];

            // compiler -> (library name -> pacakge)
            var compilerDictionary =
                new Dictionary<string, Dictionary<string, CompiledPackage>>();

            // library name -> library.
            var libraryDictionary = new Dictionary<string, CompiledLibrary>();

            ScanCompiledFileSet(compilerDictionary, libraryDictionary);

            // all libraries for specific compiler.
            {
                var list = T.List[T.Text("all libraries")];
                foreach (var compiler in compilerDictionary.Keys.OrderBy(Config.CompilerNumber))
                {
                    var id = "boost-" + compiler;
                    var compilerLibraries = compilerDictionary[compiler];
                    CreateBinaryNuspec(
                        id,
                        compiler,
                        Enumerable.Empty<Targets.ItemDefinitionGroup>(),
                        Enumerable.Empty<Nuspec.File>(),
                        compilerLibraries
                            .Keys
                            .Select(lib => SrcPackage.Dependency(lib, compiler)),
                        Optional<string>.Absent.Value);
                    list = list
                        [T.Text(" ")]
                        [A(
                            compiler,
                            id,
                            SrcPackage.CompilerVersion(Config.CompilerMap[compiler]))];
                }
                doc = doc[list];
            }

            //
            var itemDefinitionGroupList = new[]
            {
                new Targets.ItemDefinitionGroup(
                    link: new Targets.Link(
                        additionalLibraryDirectories:
                            new[]
                            {
                                Targets.PathFromThis(Targets.LibNativePath)
                            }
                    ))
            };

            // NuGet packages for each library. 
            foreach (var library in libraryDictionary)
            {
                var name = library.Key;
                var libraryId = "boost_" + name;
                var list = T.List[T.Text(name)];
                foreach (var package in library
                    .Value
                    .PackageDictionary
                    .OrderBy(p => Config.CompilerNumber(p.Key)))
                {
                    var compiler = package.Key;
                    var packageValue = package.Value;
                    var nuspecId = libraryId + "-" + compiler;
                    CreateBinaryNuspec(
                        nuspecId,
                        compiler,
                        itemDefinitionGroupList,
                        packageValue.FileList.Select(
                            f =>
                                new Nuspec.File(
                                    Path.Combine(Config.BoostDir, f),
                                    Targets.LibNativePath)
                        ),
                        SrcPackage.BoostDependency,
                        name.ToOptional());
                    list = list
                        [T.Text(" ")]
                        [A(
                            package.Key,
                            nuspecId,
                            SrcPackage.CompilerVersion(Config.CompilerMap[compiler]))];
                }
                doc = doc[list];
            }

            // release.md
            using (var file = new StreamWriter("RELEASE.md"))
            {
                doc.Write(file);
            }
        }

    }
}
