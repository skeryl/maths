using System;

namespace DataStructures
{
    public static class ExtensionMethods
    {
        private const double Epsilon = 0.000000000001;

        public static bool IsNonZero(this double d)
        {
            return !IsEqualTo(d, 0);
        }

        public static bool IsEqualTo(this double d1, double d2)
        {
            return Math.Abs(d1 - d2) < Epsilon;
        }
    }
}
