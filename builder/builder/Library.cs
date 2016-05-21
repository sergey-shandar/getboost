using Framework.G1;
using System.Collections.Generic;
using System.Linq;

namespace builder
{
    public sealed class Library
    {
        public readonly string Name;

        public readonly Optional<string> Directory;

        public readonly IEnumerable<Package> PackageList;

        public Library(
            string name,
            Optional.Class<string> directory = 
                new Optional.Class<string>(),
            Optional.Class<IEnumerable<Package>> packageList = 
                new Optional.Class<IEnumerable<Package>>())
        {
            Name = name;
            Directory = directory.Cast();
            PackageList = packageList.OneIfAbsent();
        }

        public IEnumerable<string> Create()
            => PackageList.SelectMany(p => p.Create(Directory).ToEnumerable());
    }
}
