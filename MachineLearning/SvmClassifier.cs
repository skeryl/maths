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
        
        // determines the `accuracy` of the approximation function 
        private double _epsilon = .5;
        
        // the threshold value
        private double _b = 0.0;

        private const double VerySmallValue = .00000001;

        public void Train(Vector<double>[] trainingData, string[] classLabels)
        {
            Initialize(trainingData, classLabels);
            while (UpdateNextAlphasUntilOptimized())
            {
                _ix1++;
                _ix2++;
            }
        }

        private int _ix1 = 0;
        private int _ix2 = 1;

        private bool UpdateNextAlphasUntilOptimized()
        {
            double a1_old = _a[_ix1];
            double a2_old = _a[_ix2];

            double y1 = _y[_ix1];
            double y2 = _y[_ix2];

            var x1 = _x[_ix1];
            var x2 = _x[_ix2];

            var e1 = L(_ix1) - y1;
            var e2 = L(_ix2) - y2;

            double c = (y1 * a1_old) + (y2 * a2_old);
            double s = y1 * y2;

            bool yEqual = y1.IsEqualTo(y2);

            double a1, a2;

            double low = yEqual ? Math.Max(0, a1_old + a2_old - c) : Math.Max(0, a2_old - a1_old);
            double high = yEqual ? Math.Min(c, a1_old + a2_old) : Math.Min(c, c + a2_old - a1_old);
            if (low.IsEqualTo(high))
            {
                return true;
            }

            double eta = (2 * Kernel(x1, x2)) - Kernel(x1, x1) - Kernel(x2, x2); 
            if (eta < 0)
            {
                a2 = a2_old - ((y2*(e1 - e2))/eta);
                if (a2 < low)
                {
                    a2 = low;
                }
                else if (a2 > high)
                {
                    a2 = high;
                }
                //_a[_ix2] = a2;
            }
            else
            {
                _a[_ix2] = low;
                double Lobj = L(_ix2);
                _a[_ix2] = high;
                double Hobj = L(_ix2);

                if (Lobj > (Hobj + _epsilon))
                {
                    a2 = low;
                }
                else if (Lobj < (Hobj - _epsilon))
                {
                    a2 = high;
                }
                else
                {
                    a2 = a2_old;
                }
            }
            if (a2 < VerySmallValue)
            {
                a2 = 0;
            }
            else if (a2 > (c - VerySmallValue))
            {
                a2 = c;
            }
            if (Math.Abs(a2 - a2_old) < (_epsilon*(a2 + a2_old + _epsilon)))
            {
                return false;
            }
            a1 = a1_old + (s*(a2_old - a2));
            _a[_ix1] = a1;
            _a[_ix2] = a2;
            //
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

        private double L(int i)
        {
            var x = _x[i];
            double output = 0.0;
            for (int j = 0; j < _n; j++)
            {
                output += (_a[j]*_y[j]*Kernel(_x[j], x)) + _b;
            }
            return output;
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
