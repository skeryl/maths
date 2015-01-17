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

        public override float Negate(float val)
        {
            return -val;
        }
    }
}