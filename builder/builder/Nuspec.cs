using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System;

namespace builder
{
    public static class Nuspec
    {
        private static readonly XNamespace n = XNamespace.Get(
            "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");

        private static XElement N(
            string elementName, params XAttribute[] attributeList)
            => n.Element(elementName, attributeList);

        private static XElement N(string elementName, string content)
            => N(elementName).Append(content);

        public sealed class File
        {
            private readonly string Src;

            private readonly string Target;

            public File(string src, string target)
            {
                Src = src;
                Target = target;
            }

            public XElement N
                => Nuspec.N("file", Xml.A("src", Src), Xml.A("target", Target));
        }

        public sealed class Dependency
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
            Version version,
            string description,
            IEnumerable<File> fileList,
            IEnumerable<Dependency> dependencyList,
            IEnumerable<string> tags)
        {
            var nuspec =
                N("package").Append(
                    N("metadata").Append(
                        N("id", id),
                        N("version", version.ToString()),
                        N("authors", Config.Authors),
                        N("owners", Config.Owners),
                        N("licenseUrl", "https://github.com/sergey-shandar/getboost/blob/master/LICENSE"),
                        N("projectUrl", "https://github.com/sergey-shandar/getboost"),
                        N("requireLicenseAcceptance", "false"),
                        N("description", description),
                        N("dependencies").Append(
                            dependencyList.Select(
                                d => 
                                    N(
                                        "dependency",
                                        Xml.A("id", d.Id),
                                        Xml.A("version", d.Version)
                                    )
                            )
                        ),
                        N("tags", string.Join(" ", tags))
                    ),
                    N("files").Append(fileList.Select(f => f.N))
                );
            var nuspecFile = id + ".nuspec";
            nuspec.CreateDocument().Save(nuspecFile);
            Process.Start(
                new ProcessStartInfo(
                    @"..\..\..\packages\NuGet.CommandLine.5.11.3\tools\nuget.exe", 
                    "pack " + nuspecFile)
                {
                    UseShellExecute = false,
                }).WaitForExit();
            var nupkgFile = id + "." + version + ".nupkg";
            Console.WriteLine("uploading: " + nupkgFile);
            {
                var p = Process.Start(
                  new ProcessStartInfo(
                      @"..\..\..\packages\NuGet.CommandLine.5.10.0\tools\nuget.exe",
                      $"push {nupkgFile} -Source https://api.nuget.org/v3/index.json -ApiKey {ApiKey.Value}")
                  {
                      UseShellExecute = false,
                  });
                p.WaitForExit();
                if (p.ExitCode != 0) {
                    Environment.Exit(p.ExitCode);
                }
            }
        }

        public static void Create(
            string nuspecId,
            string packageId,
            Version version,
            string description, 
            IEnumerable<Targets.ItemDefinitionGroup> itemDefinitionGroupList,
            IEnumerable<File> fileList,
            IEnumerable<CompilationUnit> compilationUnitList,
            IEnumerable<Dependency> dependencyList,
            IEnumerable<string> tags)
        {
            var unitFiles =
                compilationUnitList.
                Select(
                    u =>
                        new File(
                            u.FileName(packageId),
                            Path.Combine(Targets.SrcPath, u.LocalPath)
                        )
                );
            var targetsFile =
                Targets.Create(
                    nuspecId,
                    packageId,
                    itemDefinitionGroupList,
                    compilationUnitList);
            CreateNuspec(
                nuspecId,
                version,
                description,
                fileList.
                    Concat(unitFiles).
                    Concat(new[] { new File(targetsFile, Targets.BuildPath) }),
                dependencyList,
                new[] { "native", "nativepackage", "C++", "Boost" }.Concat(tags));
        }
    }
}
