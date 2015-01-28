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
            MachineLearningDataSet irisDataSet = Loader.LoadFromFile("../../MachineLearning/Data/iris-1-2.data");
            var svm = new LinearSvm();
            svm.Train(irisDataSet.InputData, irisDataSet.ClassLabels);
            Console.WriteLine("accuracy: {0:P}", svm.GetAccuracy());
        }
    }
}
