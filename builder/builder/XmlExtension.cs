using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace builder
{
    internal static class XmlExtension
    {
        public static XDocument CreateDocument(this XElement element)
        {
            var document = new XDocument();
            document.Add(element);
            return document;
        }

        public static XElement Append(
            this XElement element, IEnumerable<XAttribute> attributeList)
        {
            foreach (var attribute in attributeList)
            {
                element.Add(attribute);
            }
            return element;
        }

        public static XElement Append(
            this XElement element, params XAttribute[] attributeList)
        {
            return element.Append((IEnumerable<XAttribute>)attributeList);
        }

        public static XElement Append(
            this XElement element, IEnumerable<XElement> childList)
        {
            foreach (var child in childList)
            {
                element.Add(child);
            }
            return element;
        }

        public static XElement Append(
            this XElement element, params XElement[] childList)
        {
            return element.Append((IEnumerable<XElement>)childList);
        }

        public static XElement Append(this XElement element, string content)
        {
            element.Add(content);
            return element;
        }

        public static XElement Element(
            this XNamespace ns,
            string localName,
            params XAttribute[] attributeList)
        {
            var element = new XElement(ns.GetName(localName));
            foreach (var attribute in attributeList)
            {
                element.Add(attribute);
            }
            return element;
        }

        public static XAttribute Attribute(
            this XNamespace ns, string localName, string value)
        {
            return new XAttribute(ns.GetName(localName), value);
        }
    }
}
