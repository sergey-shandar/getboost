using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace builder
{
    sealed class CompiledLibrary
    {
        public readonly Dictionary<string, CompiledPackage> PackageDictionary =
            new Dictionary<string, CompiledPackage>();
    }
}
