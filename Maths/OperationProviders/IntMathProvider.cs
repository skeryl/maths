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

        public override int Negate(int val)
        {
            return -val;
        }
    }
}