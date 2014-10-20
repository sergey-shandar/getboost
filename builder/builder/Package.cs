using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace builder
{
    public sealed class Package
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
            Optional.Class<string> name = new Optional.Class<string>(),
            Optional.Class<IEnumerable<string>> preprocessorDefinitions = 
                new Optional.Class<IEnumerable<string>>(),
            Optional.Class<IEnumerable<string>> lineList = 
                new Optional.Class<IEnumerable<string>>(),
            Optional.Class<IEnumerable<string>> fileList = 
                new Optional.Class<IEnumerable<string>>(),
            bool skip = false)
        {
            Name = name.Cast();
            PreprocessorDefinitions = preprocessorDefinitions.EmptyIfAbsent();
            LineList = lineList.EmptyIfAbsent();
            FileList = fileList.EmptyIfAbsent();
            Skip = skip;
        }

        public Package() : this(Optional.Absent)
        {
        }

        public Package(string name, Package package, IEnumerable<string> fileList):
            this(
                name: name,
                preprocessorDefinitions: package.PreprocessorDefinitions.ToOptionalClass(),
                lineList: package.LineList.ToOptionalClass(),
                fileList: fileList.ToOptionalClass(),
                skip: package.Skip)
        {
        }

        private static Nuspec.Dependency Dependency(string id)
        {
            return new Nuspec.Dependency(id, "[" + Config.Version.ToString() + "]");
        }

        public static Nuspec.Dependency Dependency(string library, string compiler)
        {
            return Dependency("boost_" + library + "-" + compiler);
        }

        public static IEnumerable<Nuspec.Dependency> BoostDependency
        {
            get
            {
                return new[] { Dependency("boost") };
            }
        }

        public Optional<string> Create(Optional<string> directory)
        {
            if (!Skip)
            {
                var name = Name.Select(n => n, () => "");
                var nuspecId = name;
                var srcFiles =
                    FileList.Select(
                        f =>
                            new Nuspec.File(
                                directory.Select(d => Path.Combine(d, f), () => f),
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
                                Concat(new[] { name.ToUpper() + "_NO_LIB" }).
                                ToOptionalClass()
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
