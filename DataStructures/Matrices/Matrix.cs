using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures.Matrices
{
    public class Matrix
    {
        #region equality
        protected bool Equals(Matrix other)
        {
            if (NumColumns == other.NumColumns && NumRows == other.NumRows)
            {
                for (int i = 0; i < NumRows; i++)
                {
                    for (int j = 0; j < NumColumns; j++)
                    {
                        if (!AreEqual(this[i, j], other[i, j]))
                            return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (_matrix != null ? _matrix.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ NumColumns;
                hashCode = (hashCode*397) ^ NumRows;
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Matrix)obj);
        }
        #endregion

        private double[][] _matrix;

        private const double Epsilon = 0.000000000001;

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

        public static Matrix FromRows(params double[][] items)
        {
            int numRows = items.Length;
            int numColumns = items[0].Length;
            var matrix = new Matrix(numRows, numColumns) { _matrix = new double[numRows][] };
            for (int i = 0; i < numRows; i++)
            {
                matrix._matrix[i] = new double[numColumns];
                for (int j = 0; j < numColumns; j++)
                {
                    matrix._matrix[i][j] = items[i][j];
                }
            }
            return matrix;
        }

        public static Matrix FromColumns(params double[][] items)
        {
            int numRows = items[0].Length;
            int numColumns = items.Length;
            var matrix = new Matrix(numRows, numColumns) { _matrix = new double[numRows][] };
            for (int i = 0; i < numRows; i++)
            {
                matrix._matrix[i] = new double[numColumns];
                for (int j = 0; j < numColumns; j++)
                {
                    matrix._matrix[i][j] = items[j][i];
                }
            }
            return matrix;
        }

        public static Matrix Identity(int rows, int cols)
        {
            var matrix = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = AreEqual(i, j) ? 1.0 : 0.0;
                }
            }
            return matrix;
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

        public bool IsSquare { get { return NumRows == NumColumns; } }

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
            Matrix upper = Clone();
            Matrix lower = Identity(upper.NumColumns, upper.NumRows);
            var rowOperations = new List<RowOperation>();

            int row = 0;
            int pivotColumn = 0;
            while (row < upper.NumRows)
            {
                if (!IsNonZero(upper[row, pivotColumn]))
                {
                    for (int i = row + 1; i < upper.NumRows; i++)
                    {
                        if (IsNonZero(upper[i, pivotColumn]) || i == (upper.NumRows - 1))
                        {
                            rowOperations.Add(RowOperation.Swap(upper, row, i));
                        }
                    }
                }
                int col = 0;
                while (col < pivotColumn)
                {
                    var value = upper[row, col];
                    if (IsNonZero(value))
                    {
                        RowOperation rowOperation = null;
                        for (int i = 0; i < NumRows; i++)
                        {
                            if(i == row)
                                continue;
                            double otherRowValue = upper[i, col];
                            if (!IsNonZero(otherRowValue))
                            {
                                continue;
                            }
                            bool allToLeftZero = true;
                            for (int j = 0; j < col; j++)
                            {
                                allToLeftZero = allToLeftZero && !IsNonZero(upper[i, j]);
                            }
                            if (!allToLeftZero)
                            {
                                continue;
                            }
                            var multiplier = value / otherRowValue;
                            rowOperations.Add(rowOperation = RowOperation.AddRow(upper, row, i, -1 * multiplier));
                            lower[row, col] = multiplier;
                            break;
                        }
                        if (rowOperation == null && IsNonZero(upper[row, col]))
                        {
                            throw new InvalidOperationException(String.Format("Failed to perform LU Decomposition. No valid row operation was performed and the value at [{0}, {1}] is not zero.", row, col));
                        }
                    }
                    col++;
                }
                pivotColumn++;
                row++;
            }
            return new LuDecomposition { U = upper, L = lower };
        }

        private static bool IsNonZero(double d)
        {
            return !AreEqual(d, 0);
        }

        private static bool AreEqual(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < Epsilon;
        }

        public Matrix Clone()
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
            LuDecomposition luDecomposition = LuDecomposition();
            double detL = DiagonalProduct(luDecomposition.L);
            double detU = DiagonalProduct(luDecomposition.U);
            return detL * detU;
        }

        public static double DiagonalProduct(Matrix matrix)
        {
            double det = 1.0;
            for (int j = 0; j < matrix.NumColumns; j++)
            {
                det *= matrix[j, j];
            }
            return det;
        }

    }
}
