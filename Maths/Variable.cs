using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Maths
{
    [Serializable]
    public class Variable : IXmlSerializable
    {
        public string Name { get; set; }

        public double Value { get; set; }

        public override string ToString()
        {
            return double.IsNaN(Value) ? Name : string.Format("{0} = {1}", Name, Value);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Name", Name);
            writer.WriteAttributeString("Value", Convert.ToString(Value));
        }

        public void ReadXml(XmlReader reader)
        {
            Name = reader["Name"];
            double value;
            Value = double.TryParse(reader["Value"], out value) ? value : double.NaN;
        }

    }
}
