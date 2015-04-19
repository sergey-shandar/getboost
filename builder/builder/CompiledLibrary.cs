using System.Collections.Generic;

namespace builder
{
    sealed class CompiledLibrary
    {
        // compiler - package
        public readonly Dictionary<string, CompiledPackage> PackageDictionary =
            new Dictionary<string, CompiledPackage>();
    }
}
