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
            DirectoryInfo path,
            IEnumerable<SrcPackage> packageListConfig)
        {
            var firstPackage = packageListConfig.First();
            var dir = new Dir(path, "");
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
            Library libraryConfig, DirectoryInfo src)
        {
            var name = libraryConfig.Name;
            var id = "boost_" + name;

            return new Library(
                id,
                src,
                CreatePackageList(
                    name, src, libraryConfig.PackageList
                ).ToOptionalClass()
            ).Create();
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
            var info = Config.Compilers[compiler];
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
            var releaseNotes = new Doc();

            GenerateChangeset(releaseNotes);

            MakeHeaderOnlyPackage(releaseNotes);

            MakeSourcePackages(releaseNotes);

            // create dictionaries for binary NuGet packages.
            releaseNotes = releaseNotes[T.H1("Precompiled Libraries")];

            // compiler -> (library name -> pacakge)
            var compilers = new Dictionary<string, Dictionary<string, CompiledPackage>>();

            // library name -> library.
            var libraries = new Dictionary<string, CompiledLibrary>();

            BinaryFetcher.FetchBinariesFromStage(Config.BoostDir, compilers, libraries);

            MakePackages(releaseNotes, libraries);

            MakeAggregatePackage(releaseNotes, compilers);

            WriteReleaseFile(releaseNotes);
        }

        private static void GenerateChangeset(Doc releaseNotes)
        {
            releaseNotes = releaseNotes[T.H1("Release Notes")];
            foreach (var change in Config.Release)
            {
                releaseNotes = releaseNotes[change];
            }
        }

        private static void MakeHeaderOnlyPackage(Doc doc)
        {
            doc = doc
                [T.H1("Headers Only Libraries")]
                [T.List[A("boost", Config.Version)]];

            var path = Path.Combine(Config.BoostDir.FullName, "boost");
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
                            Path.Combine(Config.BoostDir.FullName, f),
                            Path.Combine(Targets.IncludePath, f)
                        )
                ),
                new CompilationUnit[0],
                new Nuspec.Dependency[0],
                new[] { "headers" }
            );
        }

        private static void MakeSourcePackages(Doc releaseNotes)
        {
            releaseNotes = releaseNotes[T.H1("Source Libraries")];
            var srcLibList = new List<string>();

            foreach (var directory in Directory
                .GetDirectories(Path.Combine(Config.BoostDir.FullName, "libs")))
            {
                var src = new DirectoryInfo(Path.Combine(directory, "src"));
                if (src.Exists)
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
                        releaseNotes = releaseNotes[T.List[A(
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
        }

        private static void MakePackages(
            Doc releaseNotes,
            Dictionary<string, CompiledLibrary> libraryDictionary)
        {
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
                                    Path.Combine(Config.BoostDir.FullName, f),
                                    Targets.LibNativePath)
                        ),
                        SrcPackage.BoostDependency,
                        name.ToOptional());
                    list = list
                        [T.Text(" ")]
                        [A(
                            package.Key,
                            nuspecId,
                            SrcPackage.CompilerVersion(Config.Compilers[compiler]))];
                }
                releaseNotes = releaseNotes[list];
            }
        }

        private static void MakeAggregatePackage(
            Doc releaseNotes,
            Dictionary<string, Dictionary<string, CompiledPackage>> compilerDictionary)
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
                        SrcPackage.CompilerVersion(Config.Compilers[compiler]))];
            }
            releaseNotes = releaseNotes[list];
        }

        private static void WriteReleaseFile(Doc releaseNotes)
        {
            using (var file = new StreamWriter("RELEASE.md"))
            {
                releaseNotes.Write(file);
            }
        }
    }
}
