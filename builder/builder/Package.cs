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
        public readonly Optional<string> Name;

        public readonly IEnumerable<string> PreprocessorDefinitions;

        public readonly IEnumerable<string> LineList;

        public readonly IEnumerable<string> FileList;

        public readonly bool Skip;

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
            string name = null,
            IEnumerable<string> preprocessorDefinitions = null,
            IEnumerable<string> lineList = null,
            IEnumerable<string> fileList = null,
            bool skip = false)
        {
            Name = name.FromNullable();
            PreprocessorDefinitions = preprocessorDefinitions.EmptyIfNull();
            LineList = lineList.EmptyIfNull();
            FileList = fileList.EmptyIfNull();
            Skip = skip;
        }

        public Package() : this(null)
        {
        }

        public Package(string name, Package package, IEnumerable<string> fileList):
            this(
                name: name,
                preprocessorDefinitions: package.PreprocessorDefinitions,
                lineList: package.LineList,
                fileList: fileList,
                skip: package.Skip)
        {
        }

        public static IEnumerable<Nuspec.Dependency> BoostDependency
        {
            get
            {
                return new[]
                {
                    new Nuspec.Dependency("boost", "[" + Config.Version.ToString() + "]")
                };
            }
        }

        public Optional<string> Create(string directory)
        {
            if (!Skip)
            {
                var name = Name.Select(n => n, () => "");
                var nuspecId = name;
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
                        preprocessorDefinitions:
                            PreprocessorDefinitions.
                                Concat(new[] { name.ToUpper() + "_NO_LIB" })
                    );

                Nuspec.Create(
                    nuspecId,
                    name,
                    new[] 
                    {
                        new Targets.ItemDefinitionGroup(clCompile: clCompile)
                    },
                    srcFiles,
                    CompilationUnitList,
                    BoostDependency
                );
                return Name;
            }
            else
            {
                return Optional.Absent;
            }
        }
    }
}
