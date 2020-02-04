using System.Collections.Generic;
using System.IO;

namespace builder
{
    internal class BinaryFetcher
    {
        internal static void FetchBinariesFromStage(
            DirectoryInfo boostDirectory,
            Dictionary<string, Dictionary<string, CompiledPackage>> compilers,
            Dictionary<string, CompiledLibrary> libraries)
        {
            foreach (var toolsetDirectory in boostDirectory.GetDirectories("stage/*"))
            {
                FetchBinariesFromToolsetDirectory(toolsetDirectory, compilers, libraries);
            }
        }

        private static void FetchBinariesFromToolsetDirectory(
            DirectoryInfo toolsetDirectory,
            Dictionary<string, Dictionary<string, CompiledPackage>> compilers,
            Dictionary<string, CompiledLibrary> libraries)
        {
            foreach (var dir in toolsetDirectory.GetDirectories())
            {
                FetchBinariesFromArchitectureDirectory(dir.CreateSubdirectory("lib"), compilers, libraries);
            }
        }

        private static void FetchBinariesFromArchitectureDirectory(
            DirectoryInfo architectureDirectory,
            Dictionary<string, Dictionary<string, CompiledPackage>> compilers,
            Dictionary<string, CompiledLibrary> libraries)
        {
            foreach (var file in architectureDirectory.GetFiles("*boost*"))
            {
                var split = file.Name.SplitFirst('-');
                var library = split.Before.SplitFirst('_').After;
                var compiler = split.After.SplitFirst('-').Before;

                //
                var compiledLibrary = libraries.GetOrAddNew(library);
                var compiledPackage = compiledLibrary.PackageDictionary.GetOrAddNew(compiler);
                compiledPackage.AddFile(architectureDirectory.FullName, file.Name);

                // add the compiler and add the library to the compiler.
                compilers.GetOrAddNew(compiler)[library] = compiledPackage;
            }
        }
    }
}
