using Framework.G1;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace builder
{
    public sealed class Library
    {
        public readonly string Name;

        public readonly Optional<DirectoryInfo> Directory;

        public readonly IEnumerable<SrcPackage> PackageList;

        public Library(
            string name,
            Optional.Class<DirectoryInfo> directory =
                new Optional.Class<DirectoryInfo>(),
            Optional.Class<IEnumerable<SrcPackage>> packageList =
                new Optional.Class<IEnumerable<SrcPackage>>())
        {
            Name = name;
            Directory = directory.Cast();
            PackageList = packageList.OneIfAbsent();
        }

        public IEnumerable<string> Create()
            => PackageList.SelectMany(package => package.Create(Directory).ToEnumerable());
    }
}
