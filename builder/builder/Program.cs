using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;

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
                        i => new Dir(i, Path.Combine(Name, i.Name))).
                    Where(
                        dir => filter(dir.Name)).
                    SelectMany(
                        dir => dir.FileList(filter)).
                    Concat(
                        Info.
                        GetFiles().
                        Select(f => Path.Combine(Name, f.Name)).
                        Where(f => filter(f))
                    );
            }
        }

        static IEnumerable<Package> CreatePackageList(
            string path,
            IEnumerable<Package> packageListConfig)
        {
            var firstPackage = packageListConfig.First();
            var firstFileSet = firstPackage.FileList.ToHashSet();
            var extraPackageList = packageListConfig.Skip(1);
            //
            var extraFileSet =
                extraPackageList.
                SelectMany(p => p.FileList).
                ToHashSet();
            //
            var dir = new Dir(new DirectoryInfo(path), "");
            yield return 
                new Package(
                    name: null,
                    lineList: firstPackage.LineList,
                    fileList: 
                        dir.
                        FileList(
                            s => 
                                !extraFileSet.Contains(s) || 
                                firstFileSet.Contains(s)
                        )
                );
            //
            foreach (var p in extraPackageList)
            {
                var set = p.FileList.ToHashSet();
                yield return new Package(
                    name: p.Name,
                    lineList: p.LineList,
                    fileList: dir.FileList(s => set.Contains(s)));
            }
        }

        static void MakeLibrary(Library libraryConfig, string src)
        {
            new Library(
                libraryConfig.Name,
                src,
                CreatePackageList(src, libraryConfig.PackageList)
            ).
            Create();
        }

        static void Main(string[] args)
        {
            // headers only.
            {
                var path = Path.Combine(Config.BoostDir, "boost");
                var fileList =
                    new Dir(new DirectoryInfo(path), "boost").
                    FileList(f => true);
                var clCompile =
                    Targets.M(
                        "AdditionalIncludeDirectories", 
                        Targets.PathFromThis(Targets.IncludePath) +
                            ";%(AdditionalIncludeDirectories)"
                    );
                Nuspec.Create(
                    "boost",
                    "boost",
                    clCompile,
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

                    MakeLibrary(libraryConfig, src);
                }
            }
        }

    }
}
