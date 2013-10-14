using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;

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

        public static void Run(string id, IEnumerable<File> fileList)
        {
            var versionRange =
                "[" +
                new Version(Config.Version.Major, Config.Version.Minor) +
                "," +
                new Version(Config.Version.Major, Config.Version.Minor + 1) +
                ")";
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
                            N(
                                "dependency",
                                Xml.A("id", "boost"),
                                Xml.A("version", versionRange)
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
    }
}
