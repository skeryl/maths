using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Maths
{
    [Serializable]
    public class Expression : IXmlSerializable, ISerializable
    {
        private readonly Evaluator _evaluator;

        public string OriginalInput { get; set; }
        
        [XmlIgnore]
        public Stack<object> RpnStack { get; set; }

        public Expression()
        {
            _evaluator = new Evaluator();
        }

        public Expression(string expressionToParse)
        {
            _evaluator = new Evaluator();
            var toClone = _evaluator.Parse(expressionToParse);
            OriginalInput = expressionToParse;
            RpnStack = toClone.RpnStack;
        }

        internal Expression(Evaluator evaluator, Stack<object> rpnStack, string originalInput)
        {
            RpnStack = rpnStack;
            OriginalInput = originalInput;
            _evaluator = evaluator;
        }

        public double Evaluate(dynamic inputVariables = null)
        {
            return _evaluator.EvaluateRpn(RpnStack, inputVariables);
        }

        public double Evaluate(Dictionary<string, double> inputVariableMap)
        {
            return _evaluator.EvaluateRpn(RpnStack, inputVariableMap);
        }

        public override string ToString()
        {
            if (RpnStack.HasVariables())
                return String.Format("f({0}) = {1}", String.Join(", ", RpnStack.GetVariables().Select(v => v.Name)), OriginalInput);
            return OriginalInput;
        }
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("OriginalInput", OriginalInput);
            info.AddValue("Stack", RpnStack.ToList());
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("OriginalInput", OriginalInput);
            writer.WriteStartElement("RpnStack");
            writer.WriteAttributeString("Count", RpnStack.Count.ToString(CultureInfo.InvariantCulture));
            var list = RpnStack.ToList();
            foreach (object obj in list)
            {
                var toAdd = obj.SerializeXml(true);
                writer.WriteStartElement("Item");
                writer.WriteAttributeString("type", obj.GetType().FullName);
                writer.WriteValue(toAdd);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public void ReadXml(XmlReader reader)
        {
            OriginalInput = reader["OriginalInput"];
            RpnStack = new Stack<object>();
            reader.ReadStartElement();
            int count = int.Parse(reader["Count"]);
            int ix = 0;
            if (reader.ReadToDescendant("Item"))
            {
                do
                {
                    string typeName = reader["type"];
                    //if (!string.IsNullOrEmpty(typeName))
                    {
                        string xml = reader.NodeType == XmlNodeType.Element
                                         ? reader.ReadElementContentAsString()
                                         : reader.ReadContentAsString();
                        object obj = xml.DeserializeXml(Type.GetType(typeName));
                        RpnStack.Push(obj);
                    }

                    ix++;
                } while (reader.ReadToNextSibling("Item"));
            }
            //}
            if(ix != count - 1)
                throw new SerializationException();
            reader.ReadEndElement();
        }

    }
}
