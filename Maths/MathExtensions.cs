using System;
using System.Collections.Generic;
using System.Linq;

namespace Maths
{
    public static class MathExtensions
    {

        public static Stack<T> Clone<T>(this Stack<T> stack)
        {
            return new Stack<T>(new Stack<T>(stack));
        }

        public static Stack<T> ReverseSelf<T>(this Stack<T> stack)
        {
            var tempList = new List<T>();
            while (stack.Count > 0)
            {
                tempList.Add(stack.Pop());
            }
            stack.Clear();
            foreach (T o in tempList)
            {
                stack.Push(o);
            }
            return stack;
        }

        public static IEnumerable<Variable> GetVariables<T>(this Stack<T> stack)
        {
            return stack.ToArray().OfType<Variable>();
        }

        public static bool HasVariables<T>(this Stack<T> stack)
        {
                var values = stack.GetVariables();
                return values.Any();
        }

    }
}
