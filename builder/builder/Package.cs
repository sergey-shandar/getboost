using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace builder
{
    sealed class Package
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

        public void Create(string directory)
        {
            var nuspecId = Name;
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
                u.Make(this);
            }
            //
            var clCompile =
                new Targets.ClCompile(
                    preprocessorDefinitions: Name.ToUpper() + "_NO_LIB");
            var versionRange =
                "[" +
                new Version(Config.Version.Major, Config.Version.Minor) +
                "," +
                new Version(Config.Version.Major, Config.Version.Minor + 1) +
                ")";
            Nuspec.Create(
                nuspecId,
                Name,
                new[] { new Targets.ItemDefinitionGroup(clCompile: clCompile) },
                srcFiles,
                CompilationUnitList,
                new[] { new Nuspec.Dependency("boost", versionRange) }
            );

        }
    }
}
