using System;
using DataStructures.Matrices;
using DataStructures.Matrices.Decompositions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MatrixTests
    {
        [Test]
        public void TestMatrixRowInitialization()
        {
            var matrix = Matrix.FromRows(new[] { 1.0, 2, 3 }, new[] { 4.0, 5, 6 });
            Assert.AreEqual(2, matrix.NumRows);
            Assert.AreEqual(3, matrix.NumColumns);
            Assert.AreEqual(3, matrix[0, 2]);
            Assert.AreEqual(2, matrix[0, 1]);
            Assert.AreEqual(6, matrix[1, 2]);
            Console.WriteLine(matrix);
        }

        [Test]
        public void TestMatrixColumnInitialization()
        {
            var matrix = Matrix.FromColumns(new[] { 1.0, 2, 3 }, new[] { 4.0, 5, 6 });
            Assert.AreEqual(2, matrix.NumColumns);
            Assert.AreEqual(3, matrix.NumRows);
            Assert.AreEqual(1.0, matrix[0, 0]);
            Assert.AreEqual(4, matrix[0, 1]);
            Assert.AreEqual(5, matrix[1, 1]);
            Console.WriteLine(matrix);
        }

        [Test]
        public void TestMatrixAddition()
        {
            var matrix = Matrix.FromRows(new[] { 1.0, 2, 3 }, new[] { 4.0, 5, 6 });
            var other = Matrix.FromRows(new[] { 1.0, 1, 1 }, new[] { 1.0, 1, 1 });
            var result = matrix.Add(other);
            Assert.AreEqual(result[0, 0], 2);
            Assert.AreEqual(result[0, 1], 3);
            Assert.AreEqual(result[0, 2], 4);
            Assert.AreEqual(result[1, 0], 5);
            Assert.AreEqual(result[1, 1], 6);
            Assert.AreEqual(result[1, 2], 7);
        }

        [Test]
        public void AssertMatrixAdditionDimensionalityCheck()
        {
            var matrix = Matrix.FromRows(new[] { 1.0, 2, 3 }, new[] { 4.0, 5, 6 });
            var other = Matrix.FromRows(new[] { 1.0, 1, 1 }, new[] { 1.0, 1, 1 }, new[] { 1.0, 1, 1 });
            Assert.Throws<DimensionException>(() => matrix.Add(other));
        }

        [Test]
        public void TestMatrixMultiplication()
        {
            var matrix = Matrix.FromRows(new[] { 1.0, 2, 3 }, new[] { 4.0, 5, 6 });
            var other = Matrix.FromRows(new[] { 1.0, -1, 2 }, new[] { 1.0, 2, 4 }, new[] { 3.0, 1, 5 });
            Matrix result = matrix.Multiply(other);
            Vector<double> row = result[0];
            Assert.AreEqual(row[0], 12);
            Assert.AreEqual(row[1], 6);
            Assert.AreEqual(row[2], 25);
            row = result[1];
            Assert.AreEqual(row[0], 27);
            Assert.AreEqual(row[1], 12);
            Assert.AreEqual(row[2], 58);
            Console.WriteLine(result);
        }

        [Test]
        public void TestMatrixScalarMultiplication()
        {
            var matrix = Matrix.FromRows(new[] { 1.0, 2, 3 }, new[] { 4.0, 5, 6 });
            matrix = matrix*2;
            Assert.AreEqual(2, matrix[0, 0]);
            Assert.AreEqual(8, matrix[1, 0]);
        }

        [Test]
        public void TestMatrixCloneAndEqualityComparisons()
        {
            var matrix = Matrix.FromRows(new[] { 1.0, 2, 3 }, new[] { 4.0, 5, 6 });
            Matrix otherMatrix = matrix.Clone();
            Assert.IsTrue(matrix.Equals(otherMatrix));
            Assert.AreEqual(matrix.GetHashCode(), otherMatrix.GetHashCode());

            otherMatrix[0, 0] = matrix[0, 0] + 2;
            Assert.IsFalse(matrix.Equals(otherMatrix));
            Assert.AreNotEqual(matrix.GetHashCode(), otherMatrix.GetHashCode());

            Assert.IsTrue(matrix.Equals(matrix));
        }

        [Test]
        public void TestTransposition()
        {
            var matrix = Matrix.FromRows(new[] {5.0, 5}, new[] {-1.0, 7});
            Matrix transpose = matrix.Transpose();
            Assert.AreEqual(matrix.NumRows, transpose.NumColumns);
            Assert.AreEqual(matrix.NumColumns, transpose.NumRows);
            Assert.AreEqual(matrix[0, 0], transpose[0, 0]);
            Assert.AreEqual(matrix[0, 1], transpose[1, 0]);
            Assert.AreEqual(matrix[1, 0], transpose[0, 1]);
            Assert.AreEqual(matrix[1, 1], transpose[1, 1]);
        }

        [Test]
        public void TestSvd()
        {
            var matrix = Matrix.FromRows(new[] {5.0, 5}, new[] {-1.0, 7});
            SvDecomposition svd = matrix.SvDecomposition();
        }

        [Test]
        public void TestLud()
        {
            var matrix = Matrix.FromRows(
                new[] {1.0, 0, 1}, 
                new[] {2.0, 2, 2}, 
                new[] {4.0, 4, 2});

            LuDecomposition luDecomposition = matrix.LuDecomposition();
            Assert.AreEqual(matrix, luDecomposition.L * luDecomposition.U);

            matrix = Matrix.FromRows(
                new double[] { 1, 2, 3 },
                new double[] { 2, 5, 4 },
                new double[] { 7, 2, 1 });
            luDecomposition = matrix.LuDecomposition();
            Assert.AreEqual(matrix, luDecomposition.L * luDecomposition.U);

            matrix = Matrix.FromRows(
                new double[] { 0, 2, 3 },
                new double[] { 2, 5, 4 },
                new double[] { 7, 2, 1 });
            luDecomposition = matrix.LuDecomposition();
            Assert.AreEqual(matrix, luDecomposition.L * luDecomposition.U);
        }

        [Test]
        [Ignore("This is still being worked on. It 'sort of' works. The resultant matrix from L * U is almost nearly the original matrix, just with an extra row of 0's.")]
        public void TestLudNonSquareMatrix()
        {
            var matrix = Matrix.FromRows(
                new[] { 1.0, 0, 1, 1 },
                new[] { 2.0, 2, 2, 3 },
                new[] { 4.0, 4, 2, 4 });

            LuDecomposition luDecomposition = matrix.LuDecomposition();
            Assert.AreEqual(matrix, luDecomposition.L * luDecomposition.U);
        }

        [Test]
        public void TestDeterminant()
        {
            var matrix = Matrix.FromRows(
                new[] {1.0, 2, 3},
                new[] {2.0, 5, 4},
                new[] {7.0, 2, 1});
            Assert.AreEqual(-44, matrix.Determinant());
        }

    }
}
