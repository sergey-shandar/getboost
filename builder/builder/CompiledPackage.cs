using System.Collections.Generic;
using System.IO;

namespace builder
{
    sealed class CompiledPackage
    {
        public IEnumerable<string> PlatformList
        {
            get { return _PlatformSet; }
        }

        public IEnumerable<string> FileList
        {
            get { return _FileList; }
        }

        public void AddFile(Platform platform, string file)
        {
            _PlatformSet.Add(platform.Name);
            _FileList.Add(Path.Combine(platform.Directory, file));
        }

        private readonly HashSet<string> _PlatformSet = new HashSet<string>();

        private readonly List<string> _FileList = new List<string>();
    }
}
