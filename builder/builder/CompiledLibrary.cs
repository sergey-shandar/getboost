using System.Collections.Generic;

namespace builder
{
    sealed class CompiledLibrary
    {
        public readonly Dictionary<string, CompiledPackage> PackageDictionary =
            new Dictionary<string, CompiledPackage>();
    }
}
