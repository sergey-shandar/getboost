using System.Collections.Generic;
using System.Linq;
using System.IO;
using Framework.G1;

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
            PreprocessorDefinitions = preprocessorDefinitions.SelectMany();
            LineList = lineList.SelectMany();
            FileList = fileList.SelectMany();
            Skip = skip;
        }

        public Package() : this(new Optional.Class<string>())
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

        private static Nuspec.Dependency DependencyOne(string id)
        {
            return new Nuspec.Dependency(id, "[" + Config.Version.ToString() + "]");
        }

        private static Nuspec.Dependency DependencyRange(string id)
        {
            var next = new StableVersion(
                Config.Version.Major, 
                Config.Version.Minor, 
                Config.Version.MajorRevision + 1,
                0);
            return new Nuspec.Dependency(
                id, 
                "[" + Config.Version.BaseString + "," + next.BaseString + ")");
        }

        public static Nuspec.Dependency Dependency(string library, string compiler)
        {
            return DependencyOne("boost_" + library + "-" + compiler);
        }

        public static IEnumerable<Nuspec.Dependency> BoostDependency
        {
            get
            {
                return new[] { DependencyRange("boost") };
            }
        }

        public Optional<string> Create(Optional<string> directory)
        {
            if (!Skip)
            {
                var name = Name.Select(n => n, () => "");
                var nuspecId = "boost_" + name;
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
                                Concat(new[] { nuspecId.ToUpper() + "_NO_LIB" }).
                                ToOptionalClass()
                    );

                Nuspec.Create(
                    nuspecId,
                    nuspecId,
                    nuspecId,
                    new[] 
                    {
                        new Targets.ItemDefinitionGroup(clCompile: clCompile)
                    },
                    srcFiles,
                    CompilationUnitList,
                    BoostDependency,
                    new[] { "sources", name }
                );
                return Name;
            }
            else
            {
                return Optional<string>.Absent.Value;
            }
        }
    }
}
