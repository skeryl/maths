using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures.Matrices;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class VectorTests
    {
        [Test]
        public void TestEuclideanDistance()
        {
            var vector = new Vector<int>(new [] { 1, 2, 3, 4, 5 });
            var other = new Vector<int> (new [] { 2, 3, 4, 5, 6 });
            double distance = vector.Distance(other);
            Assert.AreEqual(2.23606797749979, distance);

            var otherVector = new Vector<double>(new[] {-7.0, -4, 3.0});
            var otherOther = new Vector<double> (new[] {17.0,  6, 2.5});
            distance = otherVector.Distance(otherOther);
            Assert.AreEqual(26.004807247891687, distance);
        }

        [Test]
        public void TestEuclideanDistanceDimensionCheck()
        {
            var vector = new Vector<double>(new [] { 1.0, 2, 3, 4, 5 });
            var other = new Vector<double> (new [] { 2.0, 3, 4, 5 });
            Assert.Throws<InvalidOperationException>(() => vector.Distance(other));
        }

        [Test]
        public void TestDotProduct()
        {
            var vector = new Vector<double>(new double[] { 1, 2, 3 });
            var other = new Vector<double>(new double[] { 2, 2, 2 });
            Assert.AreEqual(12, vector.DotProduct(other));
        }

        [Test]
        public void TestDotProductDimensionCheck()
        {
            var vector = new Vector<double>(new[] { 1.0, 2, 3, 4, 5 });
            var other = new Vector<double>(new[] { 2.0, 3, 4, 5 });
            Assert.Throws<InvalidOperationException>(() => vector.DotProduct(other));
        }

        [Test]
        public void TestCloneAndEqualityCheck()
        {
            var vector = new Vector<int>(new [] { 1, 2, 3 });
            var clone = (Vector<int>) vector.Clone();
            bool equal = true;
            for (int i = 0; i < vector.Length; i++)
            {
                equal = equal && vector[i] == clone[i];
            }
            Assert.IsTrue(equal);
            Assert.IsTrue(vector.Equals(clone, EqualityComparer<int>.Default));
        }

    }
}
