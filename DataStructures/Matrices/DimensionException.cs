using System;

namespace DataStructures.Matrices
{
    public class DimensionException : Exception
    {
        public DimensionException(int rows, int cols)
            : base(String.Format("Dimensions were incorrect. Number of rows required: {0}. Number of columns required: {1}", rows, cols))
        {
        }

        public DimensionException(VectorType type, int expected)
            : base(String.Format("Dimensions were incorrect. Expected number of {0}s: {1}", type.ToString().ToLower(), expected))
        {
        }
    }
}