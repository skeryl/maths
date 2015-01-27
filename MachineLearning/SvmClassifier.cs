using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
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
            Initialize(trainingData, classLabels);
            while (UpdateNextAlphasUntilOptimized())
            {
                _a1Index++;
                _a2Index++;
            }
        }

        private int _a1Index = 0;
        private int _a2Index = 1;

        private bool UpdateNextAlphasUntilOptimized()
        {
            double a1_old = _a[_a1Index];
            double a2_old = _a[_a2Index];

            double y1 = _y[_a1Index];
            double y2 = _y[_a2Index];

            var x1 = _x[_a1Index];
            var x2 = _x[_a2Index];

            double c = (y1 * a1_old) + (y2 * a2_old);
            double s = y1 * y2;

            bool yEqual = y1.IsEqualTo(y2);

            double gamma = yEqual ? a1_old + a2_old : a1_old - a2_old;

            double a1 = _a[_a1Index] = gamma - (s * a2_old);

            double L = yEqual ? Math.Max(0, a1_old + a2_old - c) : Math.Max(0, a2_old - a1_old);
            double H = yEqual ? Math.Min(c, a1_old + a2_old) : Math.Min(c, c + a2_old - a1_old);
            if (L.IsEqualTo(H))
            {
                return true;
            }

            double eta = (2 * Kernel(x1, x2)) - Kernel(x1, x1) - Kernel(x2, x2); 
                
                /*a1 + a2_old + c 
                - (.5*(((y1*y1*(x1.DotProduct(x1))*a1)+(y2*y2*(x2.DotProduct(x2))*a2_old)+(2*(y1*y2*x1.DotProduct(x2)*a1*a2_old))) 
                + (2*()))
                )*/



            return true;
        }

        private void Initialize(Vector<double>[] trainingData, string[] classLabels)
        {
            _x = trainingData;
            _n = _x.Length;
            _a = new double[_n];
            _y = new double[_n];

            if (_n < 2)
            {
                throw new InvalidOperationException("Unable to train an SVM with only 1 feature. Please provide more data or use a 1-dimensional algorithm.");
            }

            _classLabels = classLabels;
            var classes = _classLabels.Distinct().ToArray();
            if (classes.Length > 2)
            {
                throw new NotSupportedException(
                    "SVMs with more than 2 classes have not yet been implemented. For now, please use only 2 classes.");
            }

            var classOne = classLabels[0];
            var classTwo = classLabels[1];

            _labelValueMap.Add(classOne, -1);
            _labelValueMap.Add(classTwo, 1);

            for (int i = 0; i < _n; i++)
            {
                _a[i] = Random.NextDouble();
                _y[i] = _labelValueMap[classLabels[i]];
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
