using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace builder
{
    public sealed class Platform
    {
        public readonly string Name;

        public readonly string Directory;

        public Platform(string name, string directory)
        {
            Name = name;
            Directory = directory;
        }

    }
}
