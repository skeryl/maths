using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Maths
{
    public class Evaluator
    {
        #region constants
        public const double Pi = 3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102;
        public const double E = 2.71828182845904523536028747135266249775724709369995;
        protected const double Epsilon = 0.0000000000000000000001;
        #endregion

        public Expression Parse(string infixString)
        {
            var operators = new Stack<Operator>();
            var output = new Stack<object>();
            string input = infixString.Replace(" ", "");
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                Operator op;
                if(Operator.TryGetOperator(c, out op))
                {
                    if(op is LeftParentheses)
                    {
                        operators.Push(op);
                    }
                    else
                    {
                        if(op is RightParentheses)
                        {
                            while(operators.Any() && !(operators.Peek() is LeftParentheses))
                            {
                                output.Push(operators.Pop());
                            }   
                            if(!operators.Any())
                                throw new ArgumentException("Mismatched parenthesis.");
                            operators.Pop();
                        }
                        else
                        {
                            if ((operators.Any() && !(operators.Peek() is LeftParentheses)) 
                                && (!operators.Any() || ((op.Precedence <= operators.Peek().Precedence && op.Association == Association.Left)
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
                    if(Char.IsDigit(c))
                    {
                        var builder = new StringBuilder();
                        builder.Append(c);
                        for (++i; i < input.Length; i++)
                        {
                            char d = input[i];
                            if(!(Char.IsDigit(d) || d == '.'))
                            {
                                i--;
                                break;
                            }
                            builder.Append(d);
                        }
                        double number;
                        if(!double.TryParse(builder.ToString(), out number))
                        {
                            throw new FormatException(string.Format("Unable to parse a number in the expression ('{0}').", builder));
                        }
                        output.Push(number);
                    }
                    else
                    {
                        var builder = new StringBuilder();
                        builder.Append(c);
                        for (++i; i < input.Length; i++)
                        {
                            char s = input[i];
                            if (Char.IsDigit(s) || s == '.' || !IsLatinLetter(s))
                            {
                                i--;
                                break;
                            }
                            builder.Append(s);
                        }
                        output.Push(new Variable { Name = builder.ToString(), Value = double.NaN } );
                    }
                }
            }
            while(operators.Any())
            {
                if (operators.Peek() is Parentheses)
                    throw new ArgumentException("Mismatched parenthesis.");
                output.Push(operators.Pop());
            }
            return new Expression(this, output.ReverseSelf(), infixString);
        }

        internal double EvaluateRpn(Stack<object> inputStack, dynamic inputVariables = null)
        {
            Dictionary<string, double> inputVariableMap = BuildVariableMap(inputStack.GetVariables().ToArray(), inputVariables);
            return EvaluateRpn(inputStack, inputVariableMap);
        }

        internal double EvaluateRpn(Stack<object> input, Dictionary<string, double> inputVariableMap)
        {
            var inputStack = input.Clone();
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

        protected static double GetArgValue(object arg, ref Dictionary<string, double> inputVariableMap)
        {
            var variable = arg as Variable;
            return variable != null ? inputVariableMap[variable.Name] : GetDouble(arg);
        }

        protected static bool IsLatinLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

        private Dictionary<string, double> BuildVariableMap(Variable[] stackVariables, dynamic inputVariables)
        {
            var map = new Dictionary<string, double>();
            if(inputVariables != null)
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
            if(expando != null)
            {
                return expando;
            }
            var dictionary = inputVariables as IDictionary<string, object>;
            if(dictionary != null)
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

        private Stack<object> ReplaceConstants(Stack<object> inputStack)
        {
            object[] objects = inputStack.ToArray();
            for (int i = 0; i < objects.Length; i++)
            {
                object o = objects[i];
                
                if (o.ToString().ToLower() == "pi")
                {
                    objects[i] = Pi;
                }
                
                if (o.ToString().ToLower() == "e")
                {
                    objects[i] = E;
                }
            }
            return new Stack<object>(objects);
        }

        private string ReplaceConstants(string input)
        {
            input = input.ToLower().Replace("pi", Pi.ToString());
            input = input.ToLower().Replace("e", E.ToString());
            return input;
        }

        public double EvaluateExpression(string input, dynamic inputVariables = null)
        {
            return EvaluateRpn(Parse(input).RpnStack, inputVariables);
        }

        public double EvaluateExpression(string input, Dictionary<string, double> inputVariableMap)
        {
            return EvaluateRpn(Parse(input).RpnStack, inputVariableMap);
        }
        
        /// <summary>
        /// Only works for values of x less than or equal to 1 and greater than 0 for now...
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns> 
        // TODO: make this able to work with all inputs for x
        public double Ln(double number)
        {
            if(number <= 0) throw new ArgumentOutOfRangeException();
            double answer = 0.0;
            for (double i = 1; i <= 10; i++)
            {
                answer += (1/i)*Power(-1, i + 1)*Power(number - 1, i);
            }
            return answer;
        }

        public double Power(double number, Double power)
        {
            double answer = 1.0;
            for (int i = 1; i <= power; i++)
            {
                answer *= number;
            }
            return answer;
        }

        /// <summary>
        /// Sine function.
        /// </summary>
        /// <param name="degree">the degree (in radians)</param>
        /// <returns>the sine of an input degree</returns>
        public double Sine(double degree)
        {
            double answer = 0.0;
            for (double i = 0; i < 99; i++)
            {
                answer += Power(-1, i)*Power(degree, (2*i) + 1)*(1/Factorial((2*i) + 1));
            }
            return answer;
        }

        public double Factorial(double number)
        {
            if (Math.Abs(number - 0.0000) < Epsilon) return 1;
            return number*Factorial(number - 1);
        }

    }
}
