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
                    package.FileList.Select(
                        f => 
                            new Nuspec.File(
                                Path.Combine(Directory, f),
                                Path.Combine(Targets.SrcPath, f)
                            )
                    );
                //
                foreach (var u in package.CompilationUnitList)
                {
                    u.Make(packageId, package);
                }
                //
                var pd = packageId.ToUpper() + "_NO_LIB;%(PreprocessorDefinitions)";
                var clCompile = Targets.M("PreprocessorDefinitions", pd);
                Nuspec.Create(
                    nuspecId,
                    packageId,
                    clCompile,
                    srcFiles,
                    package.CompilationUnitList
                );
            }
        }

    }
}
