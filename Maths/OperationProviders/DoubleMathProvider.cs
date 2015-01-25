using System;

namespace Maths.OperationProviders
{
    public class DoubleMathProvider : MathProvider<double>
    {
        public override double Add(double val, double other)
        {
            return val + other;
        }

        public override double Subtract(double val, double other)
        {
            return val - other;
        }

        public override double Multiply(double val, double other)
        {
            return val * other;
        }

        public override double Divide(double val, double other)
        {
            return val / other;
        }

        public override double AddDouble(double val, double other)
        {
            return Add(val, other);
        }

        public override double SubtractDouble(double val, double other)
        {
            return Subtract(val, other);
        }

        public override double MultiplyDouble(double val, double other)
        {
            return Multiply(val, other);
        }

        public override double DivideDouble(double val, double other)
        {
            return Divide(val, other);
        }

        public override double Negate(double val)
        {
            return -val;
        }

        public override double Power(double val, double power)
        {
            return Math.Pow(val, power);
        }

        public override double Sqrt(double distanceSquared)
        {
            return Math.Sqrt(distanceSquared);
        }
    }
}