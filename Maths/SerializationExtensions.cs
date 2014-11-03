using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Maths
{
    public static class SerializationExtensions
    {
        public static string SerializeXml(this object obj, bool asFragment = false)
        {
            var serializer = new XmlSerializer(obj.GetType());
            var stringBuilder = new StringBuilder();
            using (var writer = XmlWriter.Create(stringBuilder, new XmlWriterSettings{ OmitXmlDeclaration = asFragment, ConformanceLevel = asFragment ? ConformanceLevel.Fragment : ConformanceLevel.Auto}))
            {
                if (asFragment)
                {
                    writer.WriteWhitespace("");
                }
                serializer.Serialize(writer, obj);
            }
            return stringBuilder.ToString();

        }
        
        public static T DeserializeXml<T>(this string str) 
            where T : class
        {
            return DeserializeXml(str, typeof(T)) as T;
        }

        public static object DeserializeXml(this string str, Type type)
        {
            var serializer = new XmlSerializer(type);
            object obj;
            using (var textReader = new StringReader(str))
            {
                obj = serializer.Deserialize(textReader);
            }
            return obj;
        }
    }
}
