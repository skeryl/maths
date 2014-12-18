using System;

namespace Maths
{
    public class Evaluator
    {
        #region constants
        public const double Pi = 3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102;
        public const double E = 2.71828182845904523536028747135266249775724709369995;
        protected const double Epsilon = 0.0000000000000000000001;
        #endregion
 
        public double Ln(double number)
        {
            return Math.Log(number, 2);
        }

        public double Factorial(double number)
        {
            if (Math.Abs(number - 0.0000) < Epsilon) return 1;
            return number*Factorial(number - 1);
        }

    }
}
