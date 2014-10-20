using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace builder
{
    public static class Targets
    {
        public const string LibNativePath = @"lib\native\";

        public const string SrcPath = @"lib\native\src\";

        public const string IncludePath = @"lib\native\include\";

        public const string BuildPath = @"build\native\";

        public enum PrecompiledHeader
        {
            NotUsing,
            Create,
            Use,
        }

        public enum ExceptionHandling
        {
            Async,
            Sync,
            SyncCThrow,
            @false
        }

        private static void Append(
            XElement x, string name, IEnumerable<string> value)
        {
            x.Append(M(name, String.Join(";", value) + ";%(" + name + ")"));
        }

        public sealed class Link
        {
            public readonly IEnumerable<string> AdditionalLibraryDirectories;

            public Link(Optional.Class<IEnumerable<string>> additionalLibraryDirectories)
            {
                AdditionalLibraryDirectories =
                    additionalLibraryDirectories.EmptyIfAbsent();
            }

            public XElement X
            {
                get
                {
                    var link = M("Link");
                    Append(
                        link,
                        "AdditionalLibraryDirectories",
                        AdditionalLibraryDirectories);
                    return link;
                }
            }

        }

        public sealed class ClCompile
        {
            public readonly Optional<string> Include;

            public readonly Optional<PrecompiledHeader> PrecompiledHeader;

            public readonly IEnumerable<string> PreprocessorDefinitions;

            public readonly IEnumerable<string> AdditionalIncludeDirectories;

            public readonly Optional<bool> SDLCheck;

            public readonly Optional<ExceptionHandling> ExceptionHandling; 

            public ClCompile(
                Optional.Class<string> include = 
                    new Optional.Class<string>(),
                Optional.Struct<PrecompiledHeader> precompiledHeader = 
                    new Optional.Struct<PrecompiledHeader>(),
                Optional.Class<IEnumerable<string>> preprocessorDefinitions = 
                    new Optional.Class<IEnumerable<string>>(),
                Optional.Class<IEnumerable<string>> additionalIncludeDirectories = 
                    new Optional.Class<IEnumerable<string>>(),
                Optional.Struct<bool> sDLCheck = 
                    new Optional.Struct<bool>(),
                Optional.Struct<ExceptionHandling> exceptionHandling =
                    new Optional.Struct<ExceptionHandling>())
            {
                Include = include.Cast();
                PrecompiledHeader = precompiledHeader.Cast();
                PreprocessorDefinitions = preprocessorDefinitions.EmptyIfAbsent();
                AdditionalIncludeDirectories =
                    additionalIncludeDirectories.EmptyIfAbsent();
                SDLCheck = sDLCheck.Cast();
                ExceptionHandling = exceptionHandling.Cast();
            }

            public XElement X
            {
                get
                {
                    var clCompile = M("ClCompile");
                    Include.ForEach(i => clCompile.Append(Xml.A("Include", i)));
                    PrecompiledHeader.ForEach(
                        ph => 
                            clCompile.Append(
                                M("PrecompiledHeader", ph.ToString())
                            )
                    );
                    SDLCheck.ForEach(
                        c => clCompile.Append(M("SDLCheck", c.ToString()))
                    );
                    ExceptionHandling.ForEach(
                        eh => 
                            clCompile.Append(
                                M("ExceptionHandling", eh.ToString())
                            )
                    );
                    Append(
                        clCompile,
                        "PreprocessorDefinitions",
                        PreprocessorDefinitions);
                    Append(
                        clCompile, 
                        "AdditionalIncludeDirectories",
                        AdditionalIncludeDirectories);
                    return clCompile;
                }
            }
        }

        public sealed class ItemDefinitionGroup
        {
            public readonly Optional<string> Condition;

            public readonly Optional<ClCompile> ClCompile;

            public readonly Optional<Link> Link;

            public ItemDefinitionGroup(
                Optional.Class<string> condition = 
                    new Optional.Class<string>(),
                Optional.Class<ClCompile> clCompile = 
                    new Optional.Class<ClCompile>(),
                Optional.Class<Link> link =
                    new Optional.Class<Link>())
            {
                Condition = condition.Cast();
                ClCompile = clCompile.Cast();
                Link = link.Cast();
            }

            public XElement X
            {
                get
                {
                    var result = M("ItemDefinitionGroup");
                    Condition.ForEach(c => result.Append(Xml.A("Condition", c)));
                    ClCompile.ForEach(c => result.Append(c.X));
                    Link.ForEach(link => result.Append(link.X));
                    return result;
                }
            }
        }

        public static string PathFromThis(string path)
        {
            return Path.Combine(@"$(MSBuildThisFileDirectory)..\..\", path);
        }

        public static XElement M(
            string elementName, params XAttribute[] attributeList)
        {
            return m.Element(elementName, attributeList);
        }

        public static XElement M(string elementName, string content)
        {
            return M(elementName).Append(content);
        }

        private static readonly XNamespace m = XNamespace.Get(
            "http://schemas.microsoft.com/developer/msbuild/2003");

        public static string Create(
            string nuspecId,
            string packageId,
            IEnumerable<ItemDefinitionGroup> itemDefinitionGroupList,
            IEnumerable<CompilationUnit> compilationUnitList)
        {
            var srcPath = PathFromThis(SrcPath);
            var clCompileList =
                compilationUnitList.
                Select(u => u.ClCompile(packageId, srcPath).X);
            var targetsFile = nuspecId + ".targets";
            var targets =
                M("Project", Xml.A("ToolVersion", "4.0")).
                    Append(itemDefinitionGroupList.Select(g => g.X)).
                    Append(M("ItemGroup").Append(clCompileList)
                );
            targets.CreateDocument().Save(targetsFile);
            return targetsFile;
        }
    }
}
