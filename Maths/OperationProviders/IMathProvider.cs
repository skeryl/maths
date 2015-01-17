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
        T Negate(T val);
    }
}