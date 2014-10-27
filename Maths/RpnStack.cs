using System.Collections.Generic;
using System.Linq;

namespace Maths
{
    public class RpnStack : Stack<object>
    {
        public bool HasVariables
        {
            get 
            { 
                var values = GetVariables();
                return values.Any();
            }
        }

        public IEnumerable<Variable> GetVariables()
        {
            return ToArray().OfType<Variable>();
        }


        public void ReverseStack()
        {
            var tempList = new List<object>();
            while (Count > 0)
            {
                tempList.Add(Pop());
            }
            Clear();
            foreach (object o in tempList)
            {
                Push(o);
            }
        }

    }
}
