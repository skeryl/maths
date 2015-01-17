using System;
using System.Text;

namespace DataStructures.Matrices
{
    public class Matrix
    {
        private double[][] _matrix;

        #region initialization

        private void InitializeRow(double[][] items)
        {
            NumRows = items.Length;
            NumColumns = items[0].Length;
            _matrix = new double[NumRows][];
            for (int i = 0; i < NumRows; i++)
            {
                _matrix[i] = new double[NumColumns];
                for (int j = 0; j < NumColumns; j++)
                {
                    _matrix[i][j] = items[i][j];
                }
            }
        }

        private void InitializeColumn(double[][] items)
        {
            NumRows = items[0].Length;
            NumColumns = items.Length;
            _matrix = new double[NumRows][];
            for (int i = 0; i < NumRows; i++)
            {
                _matrix[i] = new double[NumColumns];
                for (int j = 0; j < NumColumns; j++)
                {
                    _matrix[i][j] = items[j][i];
                }
            }
        }

        #endregion

        public Matrix(int numRows, int numCols)
        {
            NumRows = numRows;
            NumColumns = numCols;
            _matrix = new double[NumRows][];
            for (int i = 0; i < NumRows; i++)
            {
                _matrix[i] = new double[NumColumns];
            }
        }

        public Matrix(VectorType type, params double[][] items)
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
                    colStrings[j] = _matrix[i][j].ToString();
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

        public Matrix Multiply(Matrix other)
        {
            AssertDimension(other, VectorType.Row, NumColumns);
            var result = new Matrix(NumRows, other.NumColumns);
            for (int i = 0; i < NumRows; i++)
            {
                for (int j = 0; j < other.NumColumns; j++)
                {
                    result[i, j] = GetRow(i) * other.GetColumn(j);
                }
            }
            return result;
        }

        public Vector<double> GetRow(int m)
        {
            return new Vector<double>(_matrix[m]);
        }

        public Vector<double> GetColumn(int n)
        {
            var col = new Vector<double>(NumRows);
            for (int i = 0; i < NumRows; i++)
            {
                col[i] = _matrix[i][n];
            }
            return col;
        } 

        private static void AssertDimensions(Matrix matrix, int numRows, int numColumns)
        {
            if(matrix.NumRows != numRows || matrix.NumColumns != numColumns)
            {
                throw new DimensionException(numRows, numColumns);
            }
        }

        private static void AssertDimension(Matrix matrix, VectorType type, int number)
        {
            bool correct = true;
            switch (type)
            {
                case VectorType.Column:
                    correct = (number == matrix.NumColumns);
                    break;
                case VectorType.Row:
                    correct = (number == matrix.NumRows);
                    break;
            }
            if (!correct)
            {
                throw new DimensionException(type, number);
            }
        }

        public double this[int row, int column]
        {
            get { return _matrix[row][column]; }
            set { _matrix[row][column] = value; }
        }

        public Vector<double> this[int index, VectorType type = VectorType.Row]
        {
            get { return type == VectorType.Row ? GetRow(index) : GetColumn(index); }
        }

        // m
        public int NumRows { get; protected set; }
        
        // m
        public int NumColumns { get; protected set; }

    }
}
