using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Maths
{
    [DataContract]
    public class Variable
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double Value { get; set; }

        public override string ToString()
        {
            return double.IsNaN(Value) ? Name : string.Format("{0} = {1}", Name, Value);
        }
    }
}
