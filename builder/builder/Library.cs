using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace builder
{
    sealed class Library
    {
        public readonly string Name;

        public readonly string Directory;

        public readonly IEnumerable<Package> PackageList;

        public Library(
            string name,
            string directory = null,
            IEnumerable<Package> packageList = null)
        {
            Name = name;
            Directory = directory;
            PackageList = packageList.OneIfNull();
        }

        public Library(): this(null)
        {
        }

        public void Create()
        {
            foreach (var package in PackageList)
            {
                var packageId = package.PackageId(Name);
                var nuspecId = packageId;
                var srcFiles =
                    package.FileList.Select(f => new Nuspec.File(
                        Path.Combine(Directory, f),
                        Path.Combine(Targets.SrcPath, f)));
                var unitFiles =
                    package.
                    CompilationUnitList.
                    Select(
                        u => 
                            new Nuspec.File(
                                u.FileName(packageId), 
                                Path.Combine(Targets.SrcPath, u.LocalPath)
                            )
                    );
                //
                foreach (var u in package.CompilationUnitList)
                {
                    u.Make(packageId, package);
                }
                //
                var targetsFile =
                    Targets.Create(
                        nuspecId, packageId, package.CompilationUnitList);
                Nuspec.Create(
                    nuspecId,
                    srcFiles.
                        Concat(unitFiles).
                        Concat(
                            new[] { 
                                new Nuspec.File(targetsFile, targetBuildPath)
                            })
                );
            }
        }

        private const string targetBuildPath = @"build\native\";
    }
}
