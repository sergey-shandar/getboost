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

        private static void Append(
            XElement x, string name, string value)
        {
            if (value != null)
            {
                x.Append(M(name, value + ";%(" + name + ")"));
            }
        }

        public sealed class Link
        {
            public readonly string AdditionalLibraryDirectories;

            public Link(string additionalLibraryDirectories)
            {
                AdditionalLibraryDirectories = additionalLibraryDirectories;
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

            public readonly string PreprocessorDefinitions;

            public readonly string AdditionalIncludeDirectories;

            public ClCompile(
                string include = null,
                PrecompiledHeader? precompiledHeader = null,
                string preprocessorDefinitions = null,
                string additionalIncludeDirectories = null)
            {
                Include = include;
                PrecompiledHeader = precompiledHeader;
                PreprocessorDefinitions = preprocessorDefinitions;
                AdditionalIncludeDirectories = additionalIncludeDirectories;
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
                M("Project", Xml.A("ToolVersion", "4.0")).Append(
                    itemDefinitionGroupList.Select(g => g.X)
                );
            targets.CreateDocument().Save(targetsFile);
            return targetsFile;
        }
    }
}
