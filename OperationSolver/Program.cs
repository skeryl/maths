using System;
using System.Collections.Generic;
using System.Dynamic;
using Maths;

namespace OperationSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var math = new MathUtilities();
            Console.WriteLine("Enter an expression to evaluate:");
            string input;
            while ((input = Console.ReadLine()) != "exit")
            {
                try
                {
                    if (string.IsNullOrEmpty(input))
                        continue;
                    var split = input.Split('|');
                    if (split.Length == 1)
                    {
                        Console.WriteLine(math.EvaluateExpression(input));
                    }
                    else if (split.Length > 1)
                    {
                        var expression = split[0];
                        var expandoArg = new ExpandoObject() as IDictionary<string, Object>;
                        var rawVariables = split[1];
                        var splitVariables = rawVariables.Split(',');
                        foreach (string splitVariable in splitVariables)
                        {
                            var splitVar = splitVariable.Split('=');
                            var varName = splitVar[0].Trim();
                            var varValue = splitVar[1].Trim();
                            expandoArg.Add(varName, varValue);
                        }
                        Console.WriteLine(math.EvaluateExpression(expression, (ExpandoObject)expandoArg));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred while attempting to evaluate the expression:\r\n\t{0}\r\n", e.Message);
                }
            }
        }
    }
}
