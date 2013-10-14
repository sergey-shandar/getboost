using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace builder
{
    static class Xml
    {
        private static readonly XNamespace x = XNamespace.Get("");

        public static XAttribute A(string attributeName, string value)
        {
            return x.Attribute(attributeName, value);
        }

    }
}
