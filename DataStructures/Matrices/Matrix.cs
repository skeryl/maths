using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures.Matrices
{
    public class Matrix
    {
        private double[][] _matrix;
        private const double EPSILON = 0.000000000001;

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

        public Matrix Multiply(double scalar)
        {
            for (int i = 0; i < NumRows; i++)
            {
                for (int j = 0; j < NumColumns; j++)
                {
                    this[i, j] *= scalar;
                }
            }
            return this;
        }

        public static Matrix operator * (Matrix matrix, Matrix other)
        {
            return matrix.Multiply(other);
        }

        public static Matrix operator *(Matrix matrix, double scalar)
        {
            return matrix.Multiply(scalar);
        }

        public static Matrix operator +(Matrix matrix, Matrix other)
        {
            return matrix.Add(other);
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

        public Matrix Transpose()
        {
            // swap the number of rows and number of columns
            var matrix = new Matrix(NumColumns, NumRows);

            // i will be a column index for our new matrix
            for (int i = 0; i < NumRows; i++)
            {
                // j will be our new row index
                for (int j = 0; j < NumColumns; j++)
                {
                    matrix[j, i] = this[i, j];
                }
            }
            return matrix;
        }

        public SvDecomposition SvDecomposition()
        {
            // C_t * C = V * Sigma_t * Sigma * V_t
            // C * V = U * Sigma

            Matrix Ct = Transpose();
            Matrix Ct_C = Ct * this;



            return null;
        }

        public LuDecomposition LuDecomposition()
        {
            Matrix matrix = Clone();
            var eliminationMatrices = new List<Matrix>();

            int pivotColumn = 0;
            int currentRow = 0;
            while (currentRow < matrix.NumRows)
            {
                bool pivotNonZero = IsNonZero(matrix[currentRow, pivotColumn]);
                if (pivotNonZero)
                {
                    bool allToLeftZero = true;
                    for (int j = 0; j < pivotColumn; j++)
                    {
                        allToLeftZero = allToLeftZero && !IsNonZero(matrix[currentRow, pivotColumn]);
                    }
                    throw new NotImplementedException();
                    if (allToLeftZero)
                    {
                        currentRow++;
                        pivotColumn++;
                    }
                }
            }

            return null;
        }

        private static bool IsNonZero(double d)
        {
            return !AreEqual(d, 0);
        }

        private static bool AreEqual(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < EPSILON;
        }

        private Matrix Clone()
        {
            var matrix = new Matrix(NumRows, NumColumns);
            for (int i = 0; i < NumRows; i++)
            {
                for (int j = 0; j < NumColumns; j++)
                {
                    matrix[i, j] = this[i, j];
                }
            }
            return matrix;
        }

        public double Determinant()
        {
            // make sure the matrix is square...
            AssertDimensions(this, NumRows, NumRows);

            return double.NaN;

        }

        public static class RowOperations
        {
            public static void Swap(Matrix m, int rowIndex1, int rowIndex2)
            {
                Vector<double> r1Copy = m.GetRow(rowIndex1);
                for (int j = 0; j < m.NumColumns; j++)
                {
                    m[rowIndex1, j] = m[rowIndex2, j];
                    m[rowIndex2, j] = r1Copy[j];
                }
            }

            public static void Multiply(Matrix m, int rowIndex, double value)
            {
                for (int j = 0; j < m.NumColumns; j++)
                {
                    m[rowIndex, j] *= value;
                }
            }

             public static void AddRow(Matrix m, int rowIndex, int rowToAddIx)
             {
                 for (int j = 0; j < m.NumColumns; j++)
                 {
                     m[rowIndex, j] += m[rowToAddIx, j];
                 }
             }
        }

    }

    public class SvDecomposition
    {
        public Matrix U { get; set; }
        public Matrix Sigma { get; set; }
        public Matrix Vt { get; set; }
    }

    public class LuDecomposition
    {
        public Matrix L { get; set; }
        public Matrix U { get; set; }
    }

}
