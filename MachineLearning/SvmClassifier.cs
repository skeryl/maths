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

        // number of input elements
        private int _n;

        // array of lagrangian multipliers ("alphas")
        private Dictionary<int, double> _a;
        
        // target value vector
        private double[] _y;

        // array of input vectors
        private Vector<double>[] _x;

        // array of (human friendly) class labels that correspond to the target value vector (_y)
        private string[] _classLabels;

        // a dictionary that maps the class labels to their target value representation ( "classOne" -> -1, "classTwo" -> 1 )
        private readonly Dictionary<string, double> _labelValueMap = new Dictionary<string, double>();
        
        // determines the `accuracy` of the approximation function 
        private double _epsilon = .5;
        
        // the threshold value
        private double _b;

        private double c = 0.23675;

        private const double VerySmallValue = .00000001;

        private const double _tolerance = 0.001;

        public void Train(Vector<double>[] trainingData, string[] classLabels)
        {
            Initialize(trainingData, classLabels);
        }


        private bool UpdateNextAlphasUntilOptimized(int ix1, int ix2)
        {
            double a1_old = _a[ix1];
            double a2_old = _a[ix2];

            double y1 = _y[ix1];
            double y2 = _y[ix2];

            var x1 = _x[ix1];
            var x2 = _x[ix2];

            var e1 = L(ix1) - y1;
            var e2 = L(ix2) - y2;

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
            }
            else
            {
                _a[ix2] = low;
                double lobj = L(ix2);
                _a[ix2] = high;
                double hobj = L(ix2);

                _a[ix2] = a2_old;

                if (lobj > (hobj + _epsilon))
                {
                    a2 = low;
                }
                else if (lobj < (hobj - _epsilon))
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
            _a[ix1] = a1;
            _a[ix2] = a2;

            // update the threshold to match the changes in alpha
            var b1 = _b - e1 - (y1*(a1 - a1_old)*(x1.DotProduct(x1))) - (y2*(a2 - a2_old)*(x1.DotProduct(x2)));
            var b2 = _b - e2 - (y1*(a1 - a1_old)*(x1.DotProduct(x2))) - (y2*(a2 - a2_old)*(x2.DotProduct(x2)));
            _b = (0 < a1 && a1 < c) ? b1 : (0 < a2 && a2 < c) ? b2 : (b1 + b2)/2;

            return true;
        }

        private bool ExamineExample(int ix2)
        {
            var y2 = _y[ix2];
            var a2 = _a[ix2];
            var e2 = L(ix2) - y2;
            var r2 = e2*y2;
            if ((r2 < -_tolerance && a2 < c) || (r2 > _tolerance && a2 > 0))
            {
                var nonZeroNonCAlphas = _a.Where(a => a.Value.IsNonZero() && !a.Value.IsEqualTo(c)).ToList();
                if (nonZeroNonCAlphas.Count > 1)
                {
                    int k, ix1;
                    float tmax;
                    for (k = 0, tmax = 0f, ix1 = -1; k < _n; k++)
                    {
                        if (_a[k] > 0 && _a[k] < c)
                        {
                            //float e2, temp;

                        }
                    }
                }
            }
            return false;
        }

        private void Initialize(Vector<double>[] trainingData, string[] classLabels)
        {
            _b = 0;
            _x = trainingData;
            _n = _x.Length;
            _a = new Dictionary<int, double>(_n + 1);
            _y = new double[_n];

            if (_n < 2)
            {
                throw new InvalidOperationException("Unable to train an SVM with only 1 feature. Please provide more data or use a 1-dimensional algorithm.");
            }

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

            for (int i = 0; i < _n; i++)
            {
                _a[i] = 0;
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

        /*private Vector<double> W()
        {
            throw new NotImplementedException("I'm not sure this is correct");
            var w = new Vector<double>(_n);
            for (int i = 0; i < _n; i++)
            {
                w[i] = _x[i].Norm() *_a[i] *_y[i];
            }
            return w;
        }*/

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
