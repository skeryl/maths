using System;
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
                Console.WriteLine(math.EvaluateExpression(input));
            }

        }
    }
}
