using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace builder
{
    sealed class Library
    {
        public readonly string Name;

        public readonly string Directory;

        public readonly IEnumerable<Package> PackageList;

        public Library(
            string name,
            string directory = null,
            IEnumerable<Package> packageList = null)
        {
            Name = name;
            Directory = directory;
            PackageList = packageList.OneIfNull();
        }

        public Library(): this(null)
        {
        }

        public void Create()
        {
            foreach (var package in PackageList)
            {
                var packageId = package.PackageId(Name);
                var nuspecId = packageId;
                var srcFiles =
                    package.FileList.Select(f => new Nuspec.File(
                        Path.Combine(Directory, f),
                        Path.Combine(targetSrcPath, f)));
                var unitFiles =
                    package.
                    CompilationUnitList.
                    Select(
                        u => 
                            new Nuspec.File(
                                u.FileName(packageId), 
                                Path.Combine(targetSrcPath, u.LocalPath)
                            )
                    );
                var targetsFile = nuspecId + ".targets";
                //
                foreach (var u in package.CompilationUnitList)
                {
                    u.Make(packageId, package);
                }
                //
                var pd = packageId.ToUpper() + "_NO_LIB;%(PreprocessorDefinitions)";
                var srcPath = Path.Combine(
                        @"$(MSBuildThisFileDirectory)..\..\", targetSrcPath);
                /*
                var additionalIncludeDirectories =
                    ".;%(AdditionalIncludeDirectories)";
                 * */
                var unitList = 
                    package.
                    CompilationUnitList.
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
                var targets =
                    M("Project", Xml.A("ToolVersion", "4.0")).Append(
                        M("ItemDefinitionGroup").Append(
                            M("ClCompile").Append(
                                M("PreprocessorDefinitions", pd)
                            )
                        ),
                        M("ItemGroup").Append(unitList)
                    );
                targets.CreateDocument().Save(targetsFile);
                Nuspec.Run(
                    nuspecId,
                    srcFiles.
                    Concat(unitFiles).
                    Concat(
                        new[] { 
                            new Nuspec.File(targetsFile, targetBuildPath)
                        })
                );
            }
        }

        private static XElement M(
            string elementName, params XAttribute[] attributeList)
        {
            return m.Element(elementName, attributeList);
        }

        private static XElement M(string elementName, string content)
        {
            return M(elementName).Append(content);
        }

        private static readonly XNamespace m = XNamespace.Get(
            "http://schemas.microsoft.com/developer/msbuild/2003");

        private const string targetSrcPath = @"lib\native\src\";

        private const string targetBuildPath = @"build\native\";
    }
}
