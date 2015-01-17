﻿using System;
using DataStructures.Matrices;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MatrixTests
    {
        [Test]
        public void TestMatrixInitialization()
        {
            var matrix = new Matrix(VectorType.Row, new[] { 1.0, 2, 3 }, new[] { 4.0, 5, 6 });
            Console.WriteLine(matrix);
        }

        [Test]
        public void TestMatrixAddition()
        {
            var matrix = new Matrix(VectorType.Row, new[] { 1.0, 2, 3 }, new[] { 4.0, 5, 6 });
            var other = new Matrix(VectorType.Row, new[] { 1.0, 1, 1 }, new[] { 1.0, 1, 1 });
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
            var matrix = new Matrix(VectorType.Row, new[] { 1.0, 2, 3 }, new[] { 4.0, 5, 6 });
            var other = new Matrix(VectorType.Row, new[] { 1.0, 1, 1 }, new[] { 1.0, 1, 1 }, new[] { 1.0, 1, 1 });
            Assert.Throws<DimensionException>(() => matrix.Add(other));
        }

        [Test]
        public void TestMatrixMultiplication()
        {
            var matrix = new Matrix(VectorType.Row, new[] { 1.0, 2, 3 }, new[] { 4.0, 5, 6 });
            var other = new Matrix(VectorType.Row, new[] { 1.0, -1, 2 }, new[] { 1.0, 2, 4 }, new[] { 3.0, 1, 5 });
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

    }
}
