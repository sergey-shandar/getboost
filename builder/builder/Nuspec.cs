using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;

namespace builder
{
    static class Nuspec
    {
        private static readonly XNamespace n = XNamespace.Get(
            "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");

        private static XElement N(
            string elementName, params XAttribute[] attributeList)
        {
            return n.Element(elementName, attributeList);
        }

        private static XElement N(string elementName, string content)
        {
            return N(elementName).Append(content);
        }

        public class File
        {
            private readonly string Src;

            private readonly string Target;

            public File(string src, string target)
            {
                Src = src;
                Target = target;
            }

            public XElement N
            {
                get 
                { 
                    return 
                        Nuspec.N(
                            "file", Xml.A("src", Src), Xml.A("target", Target));
                }
            }
        }

        public class Dependency
        {
            public readonly string Id;

            public readonly string Version;

            public Dependency(string id, string version)
            {
                Id = id;
                Version = version;
            }
        };

        private static void CreateNuspec(
            string id,
            IEnumerable<File> fileList,
            IEnumerable<Dependency> dependencyList)
        {
            /*
            var versionRange =
                "[" +
                new Version(Config.Version.Major, Config.Version.Minor) +
                "," +
                new Version(Config.Version.Major, Config.Version.Minor + 1) +
                ")";
             * */
            var nuspec =
                N("package").Append(
                    N("metadata").Append(
                        N("id", id),
                        N("version", Config.Version.ToString()),
                        N("authors", Config.Authors),
                        N("owners", Config.Authors),
                        N("licenseUrl", "http://www.boost.org/LICENSE_1_0.txt"),
                        N("projectUrl", "http://boost.org/"),
                        N("requireLicenseAcceptance", "false"),
                        N("description", id),
                        N("dependencies").Append(
                            dependencyList.Select(
                                d => 
                                    N(
                                        "dependency",
                                        Xml.A("id", d.Id),
                                        Xml.A("version", d.Version)
                                    )
                            )
                        )
                    ),
                    N("files").Append(fileList.Select(f => f.N))
                );
            var nuspecFile = id + ".nuspec";
            nuspec.CreateDocument().Save(nuspecFile);
            Process.Start(
                new ProcessStartInfo(
                    @"C:\programs\nuget.exe", "pack " + nuspecFile)
                {
                    UseShellExecute = false,
                }).WaitForExit();
        }

        public static void Create(
            string nuspecId,
            string packageId, 
            XElement clCompile,
            IEnumerable<File> fileList,
            IEnumerable<CompilationUnit> compilationUnitList,
            IEnumerable<Nuspec.Dependency> dependencyList)
        {
            var unitFiles =
                compilationUnitList.
                Select(
                    u =>
                        new Nuspec.File(
                            u.FileName(packageId),
                            Path.Combine(Targets.SrcPath, u.LocalPath)
                        )
                );
            var targetsFile =
                Targets.Create(
                    nuspecId, packageId, clCompile, compilationUnitList);
            CreateNuspec(
                nuspecId,
                fileList.
                    Concat(unitFiles).
                    Concat(new[] { new File(targetsFile, Targets.BuildPath) }),
                dependencyList
            );
        }
    }
}
