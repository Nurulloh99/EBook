using System.Xml;
using System.Xml.Serialization;

namespace EBook.Core.Helpers
{
    public static class XmlExtensions
    {
        public static string SerializeToXml<T>(this T @object)
        {
            using (var sw = new Utf8StringWriter())
            using (var xw = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true }))
            {
                xw.WriteStartDocument(true); // that bool parameter is called "standalone"

                var namespaces = new XmlSerializerNamespaces();

                namespaces.Add(string.Empty, "urn:LoadTests");
                var xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(xw, @object, namespaces);
                return sw.ToString();
            }
        }
    }
}
