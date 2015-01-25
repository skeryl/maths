using System;

namespace Maths.OperationProviders
{
    public class IntMathProvider : MathProvider<int>
    {
        public override int Add(int val, int other)
        {
            return val + other;
        }

        public override int Subtract(int val, int other)
        {
            return val - other;
        }

        public override int Multiply(int val, int other)
        {
            return val * other;
        }

        public override int Divide(int val, int other)
        {
            return val / other;
        }

        public override int AddDouble(int val, double other)
        {
            return Add(val, Convert.ToInt32(other));
        }

        public override int SubtractDouble(int val, double other)
        {
            return Subtract(val, Convert.ToInt32(other));
        }

        public override int MultiplyDouble(int val, double other)
        {
            return Multiply(val, Convert.ToInt32(other));
        }

        public override int DivideDouble(int val, double other)
        {
            return Divide(val, Convert.ToInt32(other));
        }

        public override int Negate(int val)
        {
            return -val;
        }

        public override double Power(int val, double power)
        {
            return (int) Math.Pow(val, power);
        }

        public override double Sqrt(int value)
        {
            return Math.Sqrt(value);
        }
    }
}