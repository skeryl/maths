using System;

namespace DataStructures.Matrices
{
    public class DimensionException : Exception
    {
        public DimensionException(int rows, int cols)
            : base(String.Format("Dimensions were incorrect. Number of rows required: {0}. Number of columns required: {1}", rows, cols))
        {
        }
    }
}