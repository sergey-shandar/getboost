using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace builder
{
    sealed class LibraryConfig
    {
        public readonly string Name;

        public LibraryConfig(string name)
        {
            Name = name;
        }
    }
}
