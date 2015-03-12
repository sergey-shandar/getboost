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
