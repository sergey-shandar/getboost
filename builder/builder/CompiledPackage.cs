using System.Collections.Generic;
using System.IO;

namespace builder
{
    sealed class CompiledPackage
    {
        public IEnumerable<string> FileList
            => _FileList;

        public void AddFile(string directory, string file)
            => _FileList.Add(Path.Combine(directory, file));

        private readonly List<string> _FileList = new List<string>();
    }
}
