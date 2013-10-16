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
            XElement clCompile,
            IEnumerable<CompilationUnit> compilationUnitList)
        {
            var srcPath = PathFromThis(SrcPath);
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
            var targetsFile = nuspecId + ".targets";
            //var pd = packageId.ToUpper() + "_NO_LIB;%(PreprocessorDefinitions)";
            var targets =
                M("Project", Xml.A("ToolVersion", "4.0")).Append(
                    M("ItemDefinitionGroup").Append(
                        M("ClCompile").Append(
                            clCompile /*M("PreprocessorDefinitions", pd) */
                        )
                ),
                M("ItemGroup").Append(unitList)
            );
            targets.CreateDocument().Save(targetsFile);
            return targetsFile;
        }
    }
}
