using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Maths
{
    [DataContract]
    [Serializable]
    public class Expression
    {
        private readonly Evaluator _evaluator;

        [DataMember]
        public string OriginalInput { get; set; }
        
        [DataMember]
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
    }
}
