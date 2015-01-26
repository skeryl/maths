using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.Matrices;

namespace MachineLearning
{
    public abstract class SvmClassifier
    {
        protected readonly Random Random = new Random(DateTime.Now.Millisecond);

        private int _n;
        private double[] _a;
        private double[] _y;
        private Vector<double>[] _x;
        private string[] _classLabels;
        private readonly Dictionary<string, double> _labelValueMap = new Dictionary<string, double>();

        public void Train(Vector<double>[] trainingData, string[] classLabels)
        {
            _x = trainingData;
            _n = _x.Length;
            _a = new double[_n];
            _y = new double[_n];
            _classLabels = classLabels;
            var classes = _classLabels.Distinct().ToArray();
            if (classes.Length > 2)
            {
                throw new NotSupportedException("SVMs with more than 2 classes have not yet been implemented. For now, please use only 2 classes.");
            }
            var classOne = classLabels[0];
            var classTwo = classLabels[1];
            _labelValueMap.Add(classOne, -1);
            _labelValueMap.Add(classTwo, 1);
            Dictionary<string, List<Vector<double>>> partitioned = classes.ToDictionary(classLabel => classLabel, classLabel => new List<Vector<double>>());
            for (int i = 0; i < _n; i++)
            {
                var vector = _x[i];
                var classLabel = classLabels[i];
                partitioned[classLabel].Add(vector);
                _a[i] = Random.NextDouble();
                _y[i] = _labelValueMap[classLabel];
            }
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {

                }
            }
        }

        private double L()
        {
            double a = SumA();
            double output = 0.0;
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    output += (_y[i] * _y[j] * Kernel(_x[i], _x[j]) * _a[i] * _a[j]);
                }
            }
            return a - (.5 * output);
        }

        private double SumA()
        {
            double sum = 0.0;
            for (int i = 0; i < _n; i++)
            {
                sum += _a[i];
            }
            return sum;
        }

        private Vector<double> W()
        {
            throw new NotImplementedException("I'm not sure this is correct");
            var w = new Vector<double>(_n);
            for (int i = 0; i < _n; i++)
            {
                w[i] = _x[i].Norm() *_a[i] *_y[i];
            }
            return w;
        }

        protected abstract double Kernel(Vector<double> xi, Vector<double> xj);
    }

    public class LinearSvmClassifier : SvmClassifier
    {
        protected override double Kernel(Vector<double> xi, Vector<double> xj)
        {
            return xi.DotProduct(xj);
        }
    }
}
