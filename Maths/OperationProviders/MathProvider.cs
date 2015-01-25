using System;
using System.Collections.Generic;

namespace Maths.OperationProviders
{
    public abstract class MathProvider<T> : IMathProvider<T> where T : struct, IComparable, IConvertible
    {
        public abstract T Add(T val, T other);
        public abstract T Subtract(T val, T other);
        public abstract T Multiply(T val, T other);
        public abstract T Divide(T val, T other);
        public abstract T AddDouble(T val, double other);
        public abstract T SubtractDouble(T val, double other);
        public abstract T MultiplyDouble(T val, double other);
        public abstract T DivideDouble(T val, double other);
        public abstract T Negate(T val);
        public abstract double Power(T val, double power);
        public abstract double Sqrt(T value);

        private static Dictionary<Type, Type> _providers = new Dictionary<Type, Type>
        {
            { typeof(int), typeof(IntMathProvider) },
            { typeof(double), typeof(DoubleMathProvider) },
            { typeof(float), typeof(FloatMathProvider) },
        };

        private static Dictionary<Type, IMathProvider> _providersCache = new Dictionary<Type, IMathProvider>();

        public static void Register(Type type)
        {
            _providers.Add(typeof(T), type);
        }

        public static IMathProvider<T> GetProvider()
        {
            var type = typeof (T);
            if (_providers.ContainsKey(type))
            {
                IMathProvider<T> provider;
                if (!_providersCache.ContainsKey(type))
                {
                    provider = (IMathProvider<T>)Activator.CreateInstance(_providers[type]);
                    _providersCache.Add(type, provider);                  
                }
                else
                {
                    provider = _providersCache[type] as IMathProvider<T>;
                }
                return provider;
            }
            throw new ArgumentException(String.Format("Type: '{0}' does not have a corresponding registered IMathProvider. To register a Type, use MathProvider<T>.Register(type);", type));
        }

    }
}