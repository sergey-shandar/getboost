using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace builder
{
    sealed class Library
    {
        public readonly string Name;

        public readonly string Directory;

        public readonly IEnumerable<Package> PackageList;

        public Library(
            string name,
            string directory = null,
            IEnumerable<Package> packageList = null)
        {
            Name = name;
            Directory = directory;
            PackageList = packageList.OneIfNull();
        }

        public Library(): this(null)
        {
        }

        public IEnumerable<string> Create()
        {
            foreach (var package in PackageList)
            {
                var name = package.Create(Directory);
                if(name != null)
                {
                    yield return name;
                }
            }
        }

    }
}
