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
        public const string SrcPath = @"lib\native\src\";

        public const string IncludePath = @"lib\native\include\";

        public const string BuildPath = @"build\native\";

        public enum PrecompiledHeader
        {
            NotUsing,
            Create,
            Use,
        }

        public class ClCompile
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

            private static void Append(
                XElement clCompile, string name, string value)
            {
                if(value != null)
                {
                    clCompile.Append(M(name, value + ";%(" + name + ")"));
                }
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
            ClCompile clCompile,
            IEnumerable<CompilationUnit> compilationUnitList)
        {
            var srcPath = PathFromThis(SrcPath);
            /*
            var unitList =
                compilationUnitList.
                Select(
                    u =>
                        M("ClCompile",
                            Xml.A(
                                "Include",
                                Path.Combine(
                                    srcPath,
                                    u.LocalPath,
                                    u.FileName(packageId))
                            )
                        ).Append(
                            M(
                                "PrecompiledHeader",
                                "NotUsing"
                            ),
                            M(
                                "AdditionalIncludeDirectories",
                                Path.Combine(srcPath, u.LocalPath) +
                                    ";%(AdditionalIncludeDirectories)"
                            )
                        )
                );
             * */
            var clCompileList =
                compilationUnitList.
                Select(
                    u =>
                        new ClCompile(
                            include:
                                Path.Combine(
                                    srcPath,
                                    u.LocalPath,
                                    u.FileName(packageId)
                                ),
                            precompiledHeader:
                                PrecompiledHeader.NotUsing,
                            additionalIncludeDirectories:
                                Path.Combine(srcPath, u.LocalPath)
                        )
                );
            var targetsFile = nuspecId + ".targets";
            var targets =
                M("Project", Xml.A("ToolVersion", "4.0")).Append(
                    M("ItemDefinitionGroup").Append(clCompile.X),
                    M("ItemGroup").Append(clCompileList.Select(u => u.X))
                );
            targets.CreateDocument().Save(targetsFile);
            return targetsFile;
        }
    }
}
