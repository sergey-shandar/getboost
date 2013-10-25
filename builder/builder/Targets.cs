using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace builder
{
    static class Targets
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
            if (value != null)
            {
                x.Append(M(name, String.Join(";", value) + ";%(" + name + ")"));
            }
        }

        public sealed class Link
        {
            public readonly IEnumerable<string> AdditionalLibraryDirectories;

            public Link(IEnumerable<string> additionalLibraryDirectories)
            {
                AdditionalLibraryDirectories =
                    additionalLibraryDirectories.EmptyIfNull();
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
            public readonly string Include;

            public readonly PrecompiledHeader? PrecompiledHeader;

            public readonly IEnumerable<string> PreprocessorDefinitions;

            public readonly IEnumerable<string> AdditionalIncludeDirectories;

            public readonly bool? SDLCheck;

            public readonly ExceptionHandling? ExceptionHandling; 

            public ClCompile(
                string include = null,
                PrecompiledHeader? precompiledHeader = null,
                IEnumerable<string> preprocessorDefinitions = null,
                IEnumerable<string> additionalIncludeDirectories = null,
                bool? sDLCheck = null,
                ExceptionHandling? exceptionHandling = null)
            {
                Include = include;
                PrecompiledHeader = precompiledHeader;
                PreprocessorDefinitions = preprocessorDefinitions.EmptyIfNull();
                AdditionalIncludeDirectories =
                    additionalIncludeDirectories.EmptyIfNull();
                SDLCheck = sDLCheck;
                ExceptionHandling = exceptionHandling;
            }

            public XElement X
            {
                get
                {
                    var clCompile = M("ClCompile");
                    if (Include != null)
                    {
                        clCompile.Append(Xml.A("Include", Include));
                    }
                    if (PrecompiledHeader != null)
                    {
                        clCompile.Append(
                            M("PrecompiledHeader", PrecompiledHeader.ToString())
                        );
                    }
                    if (SDLCheck != null)
                    {
                        clCompile.Append(
                            M("SDLCheck", SDLCheck.ToString())
                        );
                    }
                    if (ExceptionHandling != null)
                    {
                        clCompile.Append(
                            M("ExceptionHandling", ExceptionHandling.ToString())
                        );
                    }
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
            public readonly string Condition;

            public readonly ClCompile ClCompile;

            public readonly Link Link;

            public ItemDefinitionGroup(
                string condition = null,
                ClCompile clCompile = null,
                Link link = null)
            {
                Condition = condition;
                ClCompile = clCompile;
                Link = link;
            }

            public XElement X
            {
                get
                {
                    var result = M("ItemDefinitionGroup");
                    if (Condition != null)
                    {
                        result.Append(Xml.A("Condition", Condition));
                    }
                    if (ClCompile != null)
                    {
                        result.Append(ClCompile.X);
                    }
                    if (Link != null)
                    {
                        result.Append(Link.X);
                    }
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
