using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace builder
{
    class Package
    {
        public readonly string Name;

        public readonly IEnumerable<string> LineList;

        public readonly IEnumerable<string> FileList;

        public IEnumerable<CompilationUnit> CompilationUnitList
        {
            get
            {
                return
                    FileList.
                    Where(f => Path.GetExtension(f) == ".cpp").
                    Select(f => new CompilationUnit(f));
            }
        }

        public Package(
            string name,
            IEnumerable<string> lineList = null,
            IEnumerable<string> fileList = null)
        {
            Name = name;
            LineList = lineList.EmptyIfNull();
            FileList = fileList.EmptyIfNull();
        }

        public Package(): this(null)
        {
        }

        public string PackageId(string libraryName)
        {
            return "boost_" + libraryName + (Name == null ? "": "_" + Name);
        }

        public void Create(string libraryName, string directory)
        {
            var packageId = PackageId(libraryName);
            var nuspecId = packageId;
            var srcFiles =
                FileList.Select(
                    f =>
                        new Nuspec.File(
                            Path.Combine(directory, f),
                            Path.Combine(Targets.SrcPath, f)
                        )
                );
            //
            foreach (var u in CompilationUnitList)
            {
                u.Make(packageId, this);
            }
            //
            var clCompile =
                new Targets.ClCompile(
                    preprocessorDefinitions: packageId.ToUpper() + "_NO_LIB");
            var versionRange =
                "[" +
                new Version(Config.Version.Major, Config.Version.Minor) +
                "," +
                new Version(Config.Version.Major, Config.Version.Minor + 1) +
                ")";
            Nuspec.Create(
                nuspecId,
                packageId,
                clCompile,
                srcFiles,
                CompilationUnitList,
                new[] { new Nuspec.Dependency("boost", versionRange) }
            );

        }
    }
}
