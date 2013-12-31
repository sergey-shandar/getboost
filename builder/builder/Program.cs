using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using builder.Codeplex;

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
            string libraryName,
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
                    name: libraryName + "_" + p.Name,
                    package: p,
                    fileList: fileList
                );
            }
            //
            remainder.UnionWith(dir.FileList(firstPackage.FileList));
            yield return new Package(
                name: libraryName,
                package: firstPackage,
                fileList: remainder
            );
        }

        static IEnumerable<string> MakeLibrary(
            Library libraryConfig, string src)
        {
            var name = "boost_" + libraryConfig.Name;
            return new Library(
                name,
                src,
                CreatePackageList(
                    name, src, libraryConfig.PackageList
                )
            ).
            Create();
        }

        static void ScanCompiledFileSet(
            Dictionary<string, CompiledLibrary> dictionary, string stage)
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
                var compiledLibrary = dictionary.GetOrAddNew(library);
                var compiledPackage =
                    compiledLibrary.PackageDictionary.GetOrAddNew(compiler);
                compiledPackage.FileList.Add(Path.Combine(stage, file.Name));
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
                    new Nuspec.Dependency[0]
                );
            }
            // libraries.
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
                        doc = doc[T.List[A(libName.SplitFirst('_').After)]];
                    }
                }
            }
            // compiler specific libraries
            doc = doc[T.H1("Precompiled Libraries")];
            var libraryDictionary = new Dictionary<string, CompiledLibrary>();
            foreach (var platform in Config.PlatformList)
            {
                ScanCompiledFileSet(libraryDictionary, platform.Directory);
            }
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
            foreach (var library in libraryDictionary)
            {
                var libraryId = "boost_" + library.Key;
                var list = T.List[T.Text(library.Key)];
                foreach (var package in library.Value.PackageDictionary)
                {
                    var nuspecId = libraryId + "-" + package.Key;
                    Nuspec.Create(
                        nuspecId,
                        nuspecId,
                        itemDefinitionGroupList,
                        package.Value.FileList.Select(
                            f =>
                                new Nuspec.File(
                                    Path.Combine(Config.BoostDir, f),
                                    Path.Combine(Targets.LibNativePath, f)
                                )
                        ),
                        new CompilationUnit[0],
                        Package.BoostDependency
                    );
                    list = list[T.Text(" ")][A(package.Key, nuspecId)];
                }
                doc = doc[list];
            }
            using (var file = new StreamWriter("codeplex.txt"))
            {
                doc.Write(file);
            }
        }

    }
}
