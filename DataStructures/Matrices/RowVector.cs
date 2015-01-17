using System;

namespace DataStructures.Matrices
{
    public class RowVector<T> : Vector<T> 
        where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
    }

    public class ColumnVector<T> : Vector<T>
        where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
    }

}
