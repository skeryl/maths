using System;
using System.Collections;
using System.Collections.Generic;
using Maths.OperationProviders;

namespace DataStructures.Matrices
{
    public class Vector<T> : ICloneable, IEnumerable<T>, IStructuralComparable, IStructuralEquatable
        where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        protected T[] Data;

        private static readonly IMathProvider<T> MathProvider = MathProvider<T>.GetProvider();

        public Vector(int n)
        {
            Data = new T[n];
        } 

        public Vector(T[] values, bool clone = false)
        {
            Data = clone ? (T[])values.Clone() : values;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)Data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Vector<T> Add(Vector<T> other)
        {
            AssertVectorLength(other, Length);
            var result = new Vector<T>(new T[Length]);
            for (int i = 0; i < Length; i++)
            {
                result[i] = MathProvider.Add(other[i], this[i]);
            }
            return result;
        }

        public T DotProduct(Vector<T> other)
        {
            T result = default(T);
            AssertVectorLength(other, Length);
            for (int i = 0; i < Length; i++)
            {
                result = MathProvider.Add(result, (MathProvider.Multiply(Data[i], other[i])));
            }
            return result;
        }

        private void AssertVectorLength(Vector<T> vector, int length)
        {
            if (vector.Length != Length)
            {
                throw new InvalidOperationException(String.Format("Vector length expected to be: {0}.", length));
            }
        }

        public int Length
        {
            get { return Data.Length; }
        }

        public T this[int ix]
        {
            get { return Data[ix]; }
            set { Data[ix] = value; }
        }

        public static T operator *(Vector<T> vector, Vector<T> other)
        {
            return vector.DotProduct(other);
        }

        public object Clone()
        {
            return new Vector<T>((T[]) Data.Clone());
        }

        public int CompareTo(object other, IComparer comparer)
        {
            var otherVector = other as Vector<T>;
            if (otherVector != null)
            {
                return comparer.Compare(otherVector.Data, Data);
            }
            return GetHashCode().CompareTo(other.GetHashCode());
        }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            var otherVector = other as Vector<T>;
            if (otherVector != null)
            {
                bool same = true;
                if (otherVector.Length != Length)
                    return false;
                for (int i = 0; i < Length; i++)
                {
                    same = same && otherVector[i].Equals(Data[i]);
                }
                return same;
            }
            return comparer.Equals(this, other);
        }

        public double Distance(Vector<T> other)
        {
            AssertVectorLength(other, Length);
            T distanceSquared = default(T);
            for (int i = 0; i < Length; i++)
            {
                distanceSquared = MathProvider.AddDouble(distanceSquared, MathProvider.Power(MathProvider.Subtract(this[i], other[i]), 2));
            }
            return MathProvider.Sqrt(distanceSquared);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(this);
        }
    }
}
