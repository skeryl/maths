using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maths
{
    public class Function
    {
        public List<Variable> Variables { get; private set; }

        public Function()
        {

        }
        
        public double Evaluate(IEnumerable<double> args)
        {
            var variableValues = args as double[] ?? args.ToArray();
            if(variableValues.Length != Variables.Count)
                throw new ArgumentOutOfRangeException("args", "The number of supplied arguments does not match the number of required variables for this function.");
            for (int i = 0; i < Variables.Count; i++)
            {
                Variables[i].Value = variableValues[i];
            }
            return Evaluate();
        }

        private double Evaluate()
        {
            return double.NaN;
        }
    }
}
