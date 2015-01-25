using System;

namespace Maths.OperationProviders
{
    public class FloatMathProvider : MathProvider<float>
    {
        public override float Add(float val, float other)
        {
            return val + other;
        }

        public override float Subtract(float val, float other)
        {
            return val - other;
        }

        public override float Multiply(float val, float other)
        {
            return val * other;
        }

        public override float Divide(float val, float other)
        {
            return val / other;
        }

        public override float AddDouble(float val, double other)
        {
            return Add(val, (float) other);
        }

        public override float SubtractDouble(float val, double other)
        {
            return Subtract(val, (float)other);
        }

        public override float MultiplyDouble(float val, double other)
        {
            return Multiply(val, (float)other);
        }

        public override float DivideDouble(float val, double other)
        {
            return Divide(val, (float)other);
        }

        public override float Negate(float val)
        {
            return -val;
        }

        public override double Power(float val, double power)
        {
            return Math.Pow(val, power);
        }

        public override double Sqrt(float value)
        {
            return Math.Sqrt(value);
        }
    }
}