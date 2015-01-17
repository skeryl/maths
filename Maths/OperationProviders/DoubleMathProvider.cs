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

        public override double Negate(double val)
        {
            return -val;
        }
    }
}