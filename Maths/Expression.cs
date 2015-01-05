using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Maths
{
    [DataContract]
    public class Expression
    {
        private const string ExpressionRegex = "(?<operator>[()^*/+-])?(?<number>([1-9.]{1})?[0-9.]{1,})?(?<variable>[a-zA-Z]{1,})?";
        private static readonly Regex Regex = new Regex(ExpressionRegex, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        [DataMember]
        public string OriginalInput { get; set; }
        
        [DataMember]
        public Stack<object> RpnStack { get; set; }

        public Expression(string expressionToParse)
        {
            OriginalInput = expressionToParse;
            RpnStack = Parse(expressionToParse);
        }

        public static Stack<object> Parse(string infixString)
        {
            var expression = infixString.Replace(" ", "");
            var objects = ParseObjects(expression);
            var operators = new Stack<Operator>();
            var output = new Stack<object>();
            foreach (object o in objects)
            {
                var op = o as Operator;
                if (op != null)
                {
                    if (op is LeftParentheses)
                    {
                        operators.Push(op);
                    }
                    else
                    {
                        if (op is RightParentheses)
                        {
                            while (operators.Any() && !(operators.Peek() is LeftParentheses))
                            {
                                output.Push(operators.Pop());
                            }
                            if (!operators.Any())
                                throw new ArgumentException("Mismatched parenthesis.");
                            operators.Pop();
                        }
                        else
                        {
                            if ((operators.Any() && !(operators.Peek() is LeftParentheses)) && (!operators.Any() ||
                                 ((op.Precedence <= operators.Peek().Precedence && op.Association == Association.Left)
                                  || (op.Precedence < operators.Peek().Precedence && op.Association == Association.Right))))
                            {
                                output.Push(operators.Pop());
                            }
                            operators.Push(op);
                        }
                    }
                }
                else
                {
                    if (o is double)
                    {
                        output.Push((double)o);
                    }
                    else
                    {
                        output.Push(new Variable { Name = o.ToString(), Value = double.NaN });
                    }
                }
            }
            while (operators.Any())
            {
                if (operators.Peek() is Parentheses)
                    throw new ArgumentException("Mismatched parenthesis.");
                output.Push(operators.Pop());
            }
            return output.ReverseSelf();
        }

        public override string ToString()
        {
            if (RpnStack.HasVariables())
                return String.Format("f({0}) = {1}", String.Join(", ", RpnStack.GetVariables().Select(v => v.Name)), OriginalInput);
            return OriginalInput;
        }

        public double Evaluate(dynamic inputVariables = null)
        {
            Dictionary<string, double> inputVariableMap = BuildVariableMap(RpnStack.GetVariables().ToArray(), inputVariables);
            return Evaluate(inputVariableMap);
        }

        public double Evaluate(Dictionary<string, double> inputVariableMap)
        {
            var inputStack = RpnStack.Clone();
            var valStack = new Stack<object>();
            while (inputStack.Count > 0)
            {
                if (!(inputStack.Peek() is Operator))
                {
                    valStack.Push(inputStack.Pop());
                }
                else
                {
                    var op = (Operator)(inputStack.Pop());
                    var args = new List<double>();
                    for (int i = 0; i < op.NumberArguments; i++)
                    {
                        if (op is Subtraction && !valStack.Any())
                        {
                            op = new Negation();
                            break;
                        }
                        var argValue = GetArgValue(valStack.Pop(), ref inputVariableMap);
                        args.Add(argValue);
                    }
                    valStack.Push(op.Evaluate(args.ToArray()));
                }
            }
            return valStack.Count == 1 ? Convert.ToDouble(valStack.Peek()) : 0;
        }

        public List<Variable> Variables
        {
            get { return RpnStack.OfType<Variable>().Distinct(Variable.EqualityComparer).ToList(); }
        } 

        public static double EvaluateExpression(string input, dynamic inputVariables = null)
        {
            return new Expression(input).Evaluate(inputVariables);
        }

        public static double EvaluateExpression(string input, Dictionary<string, double> inputVariableMap)
        {
            return new Expression(input).Evaluate(inputVariableMap);
        }

        private static List<object> ParseObjects(string expression)
        {
            var matches = Regex.Matches(expression);
            var objectsWithIndices = new List<Tuple<int, object>>();
            foreach (Match match in matches)
            {
                Operator op;
                var operatorGroup = match.Groups["operator"];
                if (operatorGroup.Success && Operator.TryGetOperator(operatorGroup.Value.Trim().First(), out op))
                {
                    objectsWithIndices.Add(new Tuple<int, object>(operatorGroup.Index, op));
                }
                double number;
                var numberGroup = match.Groups["number"];
                if (numberGroup.Success && double.TryParse(numberGroup.Value.Trim(), out number))
                {
                    objectsWithIndices.Add(new Tuple<int, object>(operatorGroup.Index, number));
                }
                var variableGroup = match.Groups["variable"];
                if (variableGroup.Success)
                {
                    objectsWithIndices.Add(new Tuple<int, object>(operatorGroup.Index, variableGroup.Value.Trim()));
                }
            }
            return new List<object>(objectsWithIndices.OrderBy(o => o.Item1).Select(o => o.Item2));
        }

        private Dictionary<string, double> BuildVariableMap(Variable[] stackVariables, dynamic inputVariables)
        {
            var map = new Dictionary<string, double>();
            if (inputVariables != null)
            {
                var names = stackVariables.Select(s => s.Name).Distinct().ToArray();
                IDictionary<string, object> inputKeyPairs = GetInputPairs(inputVariables);
                var inputNames = inputKeyPairs.Select(s => s.Key).Distinct().ToArray();
                if (!names.All(inputNames.Contains))
                {
                    throw new ArgumentException(
                        String.Format("Not enough varibales were supplied for the function. Required arguments are ({0}) and the supplied arguments were ({1}).",
                            string.Join(", ", names), string.Join(", ", inputNames)));
                }
                foreach (var name in names)
                {
                    var variable = inputKeyPairs.FirstOrDefault(iv => iv.Key == name);
                    if (!map.ContainsKey(name))
                    {
                        map.Add(name, GetDouble(variable.Value));
                    }
                }
            }
            return map;
        }

        private IDictionary<string, object> GetInputPairs(object inputVariables)
        {
            var expando = inputVariables as ExpandoObject;
            if (expando != null)
            {
                return expando;
            }
            var dictionary = inputVariables as IDictionary<string, object>;
            if (dictionary != null)
            {
                return dictionary;
            }
            Type type = inputVariables.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var result = new Dictionary<string, object>();
            foreach (PropertyInfo property in properties)
            {
                result.Add(property.Name, property.GetValue(inputVariables, null));
            }
            return result;
        }

        private static double GetDouble(object obj)
        {
            var s = obj as string;
            if (s != null)
            {
                return double.Parse(s);
            }
            if (obj is char)
            {
                return double.Parse(obj.ToString());
            }
            return Convert.ToDouble(obj);
        }

        private static double GetArgValue(object arg, ref Dictionary<string, double> inputVariableMap)
        {
            var variable = arg as Variable;
            return variable != null ? inputVariableMap[variable.Name] : GetDouble(arg);
        }

    }
}
