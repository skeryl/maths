using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataUtilities;
using MachineLearning;
using NUnit.Framework;

namespace Tests.MachineLearning
{
    [TestFixture]
    public class SvmTests
    {
        [Test]
        public void TestLinearSvm_IrisData()
        {
            bool failed = false;
            try
            {
                MachineLearningDataSet irisDataSet = Loader.LoadFromFile("../../MachineLearning/Data/iris-1-2.data");
                var svm = new LinearSvm();
                svm.Train(irisDataSet.InputData, irisDataSet.ClassLabels);
                var accuracy = svm.GetAccuracy();
                Console.WriteLine("accuracy: {0:P}", accuracy);
                Assert.IsTrue(accuracy > 0.5, "Accuracy of the trained SVM was less than or equal to 0.5.");
            }
            catch
            {
                failed = true;
            }
            Assert.IsFalse(failed, "An exception was thrown when attempting to classify the data.");
        }

        [Test]
        public void TestNonLinearSvm_IrisData()
        {
            bool failed = false;
            try
            {
                MachineLearningDataSet irisDataSet = Loader.LoadFromFile("../../MachineLearning/Data/iris-2-3.data");
                var svm = new RbfSvm();
                svm.Train(irisDataSet.InputData, irisDataSet.ClassLabels);
                var accuracy = svm.GetAccuracy();
                Console.WriteLine("accuracy: {0:P}", accuracy);
                Assert.IsTrue(accuracy > 0.5, "Accuracy of the trained SVM was less than or equal to 0.5.");
            }
            catch
            {
                failed = true;
            }
            Assert.IsFalse(failed, "An exception was thrown when attempting to classify the data.");
        }
    }
}
