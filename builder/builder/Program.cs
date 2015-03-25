using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using builder.Codeplex;
using Framework.G1;

namespace builder
{
    class Program
    {
        class Dir
        {
            private readonly DirectoryInfo Info;
            private readonly string Name;

            public Dir(DirectoryInfo info, string name)
            {
                Info = info;
                Name = name;
            }

            public IEnumerable<string> FileList(Func<string, bool> filter)
            {
                return
                    Info.
                    GetDirectories().
                    Select(
                        i => new Dir(i, Path.Combine(Name, i.Name))
                    ).
                    SelectMany(
                        dir =>
                            filter(dir.Name) ?
                                dir.FileList() :
                                dir.FileList(filter)
                    ).
                    Concat(
                        Info.
                        GetFiles().
                        Select(f => Path.Combine(Name, f.Name)).
                        Where(f => filter(f))
                    );
            }

            public IEnumerable<string> FileList(IEnumerable<string> filter)
            {
                var set = filter.ToHashSet();
                return FileList(name => set.Contains(name));
            }

            public IEnumerable<string> FileList()
            {
                return FileList(name => true);
            }
        }

        static IEnumerable<Package> CreatePackageList(
            string name,
            string path,
            IEnumerable<Package> packageListConfig)
        {
            var firstPackage = packageListConfig.First();
            var dir = new Dir(new DirectoryInfo(path), "");
            var remainder = dir.FileList().ToHashSet();
            foreach (var p in packageListConfig.Skip(1))
            {
                var fileList = dir.FileList(p.FileList);
                remainder.ExceptWith(fileList);
                yield return new Package(
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
            yield return new Package(
                name: name,
                package: firstPackage,
                fileList: remainder
            );
        }

        static IEnumerable<string> MakeLibrary(
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

        static void ScanCompiledFileSet(
            Dictionary<string, HashSet<string>> compilerDictionary,
            Dictionary<string, CompiledLibrary> libraryDictionary,
            string stage)
        {
            foreach (
                var file in
                    new DirectoryInfo(
                        Path.Combine(Config.BoostDir, stage)
                    ).
                    GetFiles()
            )
            {
                var split = file.Name.SplitFirst('-');
                var library = split.Before.SplitFirst('_').After;
                var compiler = split.After.SplitFirst('-').Before;
                //
                var compiledLibrary = libraryDictionary.GetOrAddNew(library);
                var compiledPackage =
                    compiledLibrary.PackageDictionary.GetOrAddNew(compiler);
                compiledPackage.FileList.Add(Path.Combine(stage, file.Name));
                // add the compiler and add the library to the compiler.
                compilerDictionary.GetOrAddNew(compiler).Add(library);
            }

        }

        static A A(string name, string library)
        {
            return T.A(
                name,
                "http://nuget.org/packages/" + 
                    library + 
                    "/" + 
                    Config.Version);
        }

        static A A(string url)
        {
            return A(url, url);
        }

        static void CreateBinaryNuspec(
            string id, 
            string compiler, 
            IEnumerable<Targets.ItemDefinitionGroup> itemDefinitionGroupList,
            IEnumerable<Nuspec.File> fileList,
            IEnumerable<Nuspec.Dependency> dependencyList,
            Optional<string> name)
        {
            Nuspec.Create(
                id,
                id,
                id + ", " + Config.CompilerMap[compiler],
                itemDefinitionGroupList,
                fileList,
                Enumerable.Empty<CompilationUnit>(),
                dependencyList,
                new[] { "binaries", compiler }.Concat(name.ToEnumerable()));
        }

        static void Main(string[] args)
        {
            var doc = new Codeplex.Doc();
            // Changes
            {
                doc = doc[T.H1("Release Notes")];
                foreach(var change in Config.Release)
                {
                    doc = doc[change];
                }
            }
            // headers only library.
            {
                doc = doc
                    [T.H1("Headers Only Libraries")]
                    [T.List[A("boost")]];
                var path = Path.Combine(Config.BoostDir, "boost");
                var fileList =
                    new Dir(new DirectoryInfo(path), "boost").
                    FileList(f => true);
                Nuspec.Create(
                    "boost",
                    "boost",
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
            foreach (
                var directory in
                    Directory.GetDirectories(
                        Path.Combine(Config.BoostDir, "libs")))
            {
                var src = Path.Combine(directory, "src");
                if (Directory.Exists(src))
                {
                    var name = Path.GetFileName(directory);

                    var libraryConfig =
                        Config.LibraryList.
                        Where(l => l.Name == name).
                        FirstOrDefault() ??
                        new Library(name);

                    foreach(var libName in MakeLibrary(libraryConfig, src))
                    {
                        doc = doc[T.List[A(libName, "boost_" + libName)]];
                    }
                }
            }
            // compiler specific libraries
            doc = doc[T.H1("Precompiled Libraries")];
            var compilerDictionary = new Dictionary<string, HashSet<string>>(); 
            var libraryDictionary = new Dictionary<string, CompiledLibrary>();
            foreach (var platform in Config.PlatformList)
            {
                ScanCompiledFileSet(
                    compilerDictionary, libraryDictionary, platform.Directory);
            }
            //
            {
                var list = T.List[T.Text("all libraries")];
                foreach (var compiler in compilerDictionary.Keys)
                {
                    var id = "boost-" + compiler;
                    CreateBinaryNuspec(
                        id,
                        compiler,
                        Enumerable.Empty<Targets.ItemDefinitionGroup>(),
                        Enumerable.Empty<Nuspec.File>(),
                        compilerDictionary[compiler].
                            Select(lib => Package.Dependency(lib, compiler)),
                        Optional<string>.Absent.Value);
                    list = list[T.Text(" ")][A(compiler, id)];
                }
                doc = doc[list];
            }
            //
            var itemDefinitionGroupList = 
                Config.
                PlatformList.
                Select(
                    p => 
                        new Targets.ItemDefinitionGroup(
                            condition: "'$(Platform)'=='" + p.Name + "'",
                            link:
                                new Targets.Link(
                                    additionalLibraryDirectories:
                                        new[]
                                        {
                                            Targets.PathFromThis(
                                                Path.Combine(
                                                    Targets.LibNativePath,
                                                    p.Directory)
                                            )
                                        }
                                )
                        )
                );
            // 
            foreach (var library in libraryDictionary)
            {
                var name = library.Key;
                var libraryId = "boost_" + name;
                var list = T.List[T.Text(name)];
                foreach (var package in library.Value.PackageDictionary)
                {
                    var compiler = package.Key;
                    var nuspecId = libraryId + "-" + compiler;
                    CreateBinaryNuspec(
                        nuspecId,
                        compiler,
                        itemDefinitionGroupList,
                        package.Value.FileList.Select(
                            f =>
                                new Nuspec.File(
                                    Path.Combine(Config.BoostDir, f),
                                    Path.Combine(Targets.LibNativePath, f)
                                )
                        ),
                        Package.BoostDependency,
                        name.ToOptional());
                    list = list[T.Text(" ")][A(package.Key, nuspecId)];
                }
                doc = doc[list];
            }
            // codeplex.txt
            using (var file = new StreamWriter("codeplex.txt"))
            {
                doc.Write(file);
            }
        }
           
    }
}
