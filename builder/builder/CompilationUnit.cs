using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace builder
{
    sealed class CompilationUnit
    {
        public readonly string LocalFile;

        public CompilationUnit(string localFile)
        {
            LocalFile = localFile;
        }

        public CompilationUnit(): this(null)
        {
        }

        public string LocalPath
        {
            get { return Path.GetDirectoryName(LocalFile); }
        }

        public string FileName(string packageId)
        {
            return
                packageId + 
                "." +
                LocalFile.Replace('\\', '.');
        }

        public Targets.ClCompile ClCompile(string packageId, string srcPath)
        {
            return 
                new Targets.ClCompile(
                    include:
                        Path.Combine(
                            srcPath,
                            LocalPath,
                            FileName(packageId)
                        ),
                    precompiledHeader:
                        Targets.PrecompiledHeader.NotUsing,
                    additionalIncludeDirectories:
                        new[] { Path.Combine(srcPath, LocalPath) },
                    sDLCheck: 
                        false,
                    exceptionHandling:
                        Targets.ExceptionHandling.Async
                );
        }

        public void Make(Package package)
        {
            File.WriteAllLines(
                FileName(package.Name.Select(n => n, () => "")),
                new[]
                    {
                        "#define _SCL_SECURE_NO_WARNINGS",
                        "#define _CRT_SECURE_NO_WARNINGS",
                        "#pragma warning(disable: 4244 4503 4752 4800 4996)"
                    }.
                    Concat(package.LineList).
                    Concat(
                        new[]
                        { 
                            "#include \"" + Path.GetFileName(LocalFile) + "\""
                        }
                    )
            );
        }
    }
}
