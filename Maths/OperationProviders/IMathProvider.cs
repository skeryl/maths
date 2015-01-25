using System;

namespace Maths.OperationProviders
{
    public interface IMathProvider
    {
    }

    public interface IMathProvider<T> : IMathProvider 
        where T : struct, IComparable, IConvertible
    {
        T Add(T val, T other);
        T Subtract(T val, T other);
        T Multiply(T val, T other);
        T Divide(T val, T other);
        T AddDouble(T val, double other);
        T SubtractDouble(T val, double other);
        T MultiplyDouble(T val, double other);
        T DivideDouble(T val, double other);
        T Negate(T val);
        double Power(T val, double power);
        double Sqrt(T value);
    }
}