using System;
using System.Text;

namespace DataStructures.Matrices
{
    public class Matrix
    {
        private double[,] _matrix;

        #region initialization

        private void InitializeRow(int[][] items)
        {
            NumRows = items.Length;
            NumColumns = items[0].Length;
            _matrix = new double[NumRows, NumColumns];
            for (int i = 0; i < NumRows; i++)
            {
                for (int j = 0; j < NumColumns; j++)
                {
                    _matrix[i, j] = items[i][j];
                }
            }
        }

        private void InitializeColumn(int[][] items)
        {
            NumRows = items[0].Length;
            NumColumns = items.Length;
            _matrix = new double[NumRows, NumColumns];
            for (int i = 0; i < NumRows; i++)
            {
                for (int j = 0; j < NumColumns; j++)
                {
                    _matrix[i, j] = items[j][i];
                }
            }
        }

        #endregion

        public Matrix(int numRows, int numCols)
        {
            NumRows = numRows;
            NumColumns = numCols;
            _matrix = new double[NumRows, NumColumns];
        }

        public Matrix(VectorType type, params int[][] items)
        {
            switch (type)
            {
                case VectorType.Row:
                    InitializeRow(items);
                    break;
                case VectorType.Column:
                    InitializeColumn(items);
                    break;
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < NumRows; i++)
            {
                var colStrings = new string[NumColumns];
                for (int j = 0; j < NumColumns; j++)
                {
                    colStrings[j] = _matrix[i, j].ToString();
                }
                builder.AppendLine(String.Format("[{0}]", string.Join(", ", colStrings)));
            }
            return builder.ToString();
        }

        public Matrix Add(Matrix other)
        {
            AssertDimensions(other, NumRows, NumColumns);
            var result = new Matrix(NumRows, NumColumns);
            for (int i = 0; i < NumRows; i++)
            {
                for (int j = 0; j < NumColumns; j++)
                {
                    result[i, j] = this[i, j] + other[i, j];
                }
            }
            return result;
        }

        private static void AssertDimensions(Matrix matrix, int numRows, int numColumns)
        {
            if(matrix.NumRows != numRows || matrix.NumColumns != numColumns)
            {
                throw new DimensionException(numRows, numColumns);
            }
        }

        public double this[int row, int column]
        {
            get { return _matrix[row, column]; }
            set { _matrix[row, column] = value; }
        }

        // n
        public int NumRows { get; protected set; }
        
        // m
        public int NumColumns { get; protected set; }
    }
}
