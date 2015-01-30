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
        protected Vector<double>[] _x;

        // array of (human friendly) class labels that correspond to the target value vector (_y)
        private string[] _classLabels;

        // a dictionary that maps the class labels to their target value representation ( "classOne" -> -1, "classTwo" -> 1 )
        private readonly Dictionary<string, double> _labelValueMap = new Dictionary<string, double>();
        
        // determines the `accuracy` of the approximation function 
        private double _epsilon = .05;
        
        // the threshold value
        protected double _b;

        // weight vector; used by Linear SVMs only.
        protected Vector<double> _w;

        private double c = .5;

        private bool _isBinary;
        private bool _isSparse;

        // number of dimensions in the input vector
        protected int _d;

        private const double VerySmallValue = .00000001;

        private const double Tolerance = 0.001;

        protected abstract bool IsLinear { get; }
        
        public void Train(Vector<double>[] trainingData, string[] classLabels, bool isBinary = false, bool isSparse = false)
        {
            Initialize(trainingData, classLabels, isBinary, isSparse);
            bool changedAny = false;
            bool examineAll = true;
            while ((changedAny || examineAll))
            {
                changedAny = false;
                if (examineAll)
                {
                    for (int k = 0; k < _n; k++)
                    {
                        changedAny = changedAny || ExamineExample(k);
                    }
                }
                else
                {
                    for (int k = 0; k < _n; k++)
                    {
                        if (_a[k].IsNonZero() && !_a[k].IsEqualTo(c))
                        {
                            changedAny = changedAny || ExamineExample(k);
                        }
                    }
                }
                if (examineAll)
                {
                    examineAll = false;
                }
                else if (!changedAny)
                {
                    examineAll = true;
                }
            }
        }

        public double GetAccuracy()
        {
            double correct = 0.0;
            for (int i = 0; i < _n; i++)
            {
                double li = LearnedFunction(_x[i]);
                if (li > 0 == _y[i] > 0)
                {
                    correct++;
                }
            }
            return correct/_n;
        }

        private bool TakeStep(int ix1, int ix2)
        {
            if (ix1 == ix2)
                return false;

            double a1_old = _a[ix1];
            double a2_old = _a[ix2];

            double y1 = _y[ix1];
            double y2 = _y[ix2];

            var x1 = _x[ix1];
            var x2 = _x[ix2];

            var e1 = LearnedFunction(_x[ix1]) - y1;
            var e2 = LearnedFunction(_x[ix2]) - y2;

            double s = y1 * y2;
            
            double a1, a2;

            double low, high;

            if (y1.IsEqualTo(y2))
            {
                double gamma = a1_old + a2_old;
                if (gamma > c)
                {
                    low = gamma - c;
                    high = c;
                }
                else
                {
                    low = 0;
                    high = gamma;
                }
            }
            else
            {
                double gamma = a1_old - a2_old;
                if (gamma > 0)
                {
                    low = 0;
                    high = c - gamma;
                }
                else
                {
                    low = -gamma;
                    high = c;
                }
            }
            
            if (low.IsEqualTo(high))
            {
                return false;
            }
            double  k11 = Kernel(x1, x1), 
                    k12 = Kernel(x1, x2), 
                    k22 = Kernel(x2, x2);
            double eta = (2 * k12) - k11 - k22; 
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
                double c1 = eta/2;
                double c2 = (y2*(e1 - e2)) - (eta*a2_old);

                double lobj = (c1*low*low) + (c2*low);
                double hobj = (c1*high*high) + (c2*high);

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
            /*var b1 = _b - e1 - (y1*(a1 - a1_old)*(x1.DotProduct(x1))) - (y2*(a2 - a2_old)*(x1.DotProduct(x2)));
            var b2 = _b - e2 - (y1*(a1 - a1_old)*(x1.DotProduct(x2))) - (y2*(a2 - a2_old)*(x2.DotProduct(x2)));
            _b = (0 < a1 && a1 < c) ? b1 : (0 < a2 && a2 < c) ? b2 : (b1 + b2)/2;*/

            double b1, b2, bnew;

            b1 = _b + e1 + y1 * (a1 - a1_old) * k11 + y2 * (a2 - a2_old) * k12;
            b2 = _b + e2 + y1 * (a1 - a1_old) * k12 + y2 * (a2 - a2_old) * k22;
            if (a1 > 0 && a1 < c)
            {
                bnew = b1;
            }
            else
            {
                if (a2 > 0 && a2 < c)
                {
                    bnew = b2;
                }
                else
                {
                    bnew = (b1 + b2)/2;
                }
            }
            _b = bnew;

            if (IsLinear)
            {
                UpdateW(ix1, ix2, y1, y2, a1, a2, a1_old, a2_old);
            }

            return true;
        }

        private void UpdateW(int i1, int i2, double y1, double y2, double a1, double a2, double a1_old, double a2_old)
        {
            double t1 = y1 * (a1 - a1_old);
            double t2 = y2 * (a2 - a2_old);
            for (int i = 0; i < _d; i++)
            {
                _w[i] += ((_x[i1][i]*t1) + (_x[i2][i]*t2));
            }
        }

        private bool ExamineExample(int ix2)
        {
            var y2 = _y[ix2];
            var a2 = _a[ix2];
            var e2 = LearnedFunction(_x[ix2]) - y2;
            var r2 = e2*y2;
            if ((r2 < -Tolerance && a2 < c) || (r2 > Tolerance && a2 > 0))
            {
                var nonZeroNonCAlphas = _a.Where(a => a.Value.IsNonZero() && !a.Value.IsEqualTo(c)).ToList();
                int k, ix1;
                double tmax;
                for (k = 0, tmax = 0f, ix1 = -1; k < _n; k++)
                {
                    if (_a[k] > 0 && _a[k] < c)
                    {
                        double e1 = Math.Abs(LearnedFunction(_x[k]) - _y[k]);
                        if (e1 > tmax)
                        {
                            tmax = e1;
                            ix1 = k;
                        }
                    }
                }
                if (ix1 >= 0)
                {
                    if (TakeStep(ix1, ix2))
                        return true;
                }

                foreach (var alpha in nonZeroNonCAlphas)
                {
                    ix1 = alpha.Key;
                    if (TakeStep(ix1, ix2))
                        return true;
                }

                int kStart = Random.Next(0, _n);
                for (k = kStart; k < _n + kStart; k++)
                {
                    ix1 = k % _n;
                    if (TakeStep(ix1, ix2))
                        return true;
                }
            }
            return false;
        }

        private void Initialize(Vector<double>[] trainingData, string[] classLabels, bool isBinary, bool isSparse)
        {
            _isBinary = isBinary;
            _isSparse = isSparse;

            _b = 0;
            _x = trainingData;
            _n = _x.Length;
            _d = _x[0].Length;
            _w = new Vector<double>(_d);
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

            var classOne = classes[0];
            var classTwo = classes[1];

            _labelValueMap.Clear();
            _labelValueMap.Add(classOne, -1);
            _labelValueMap.Add(classTwo, 1);

            for (int i = 0; i < _n; i++)
            {
                _a[i] = 0;
                _y[i] = _labelValueMap[classLabels[i]];
            }
        }

        protected virtual double LearnedFunction(Vector<double> input)
        {
            double output = 0.0;
            for (int j = 0; j < _n; j++)
            {
                output += Kernel(input, _x[j]) * _a[j];
            }
            output -= _b;
            return output;
        }

        protected abstract double Kernel(Vector<double> xi, Vector<double> xj);
    }

    public class LinearSvm : SvmClassifier
    {
        protected override bool IsLinear
        {
            get { return true; }
        }

        protected override double LearnedFunction(Vector<double> input)
        {
            double s = 0;
            for (int i = 0; i < _d; i++)
            {
                s += _w[i]*input[i];
            }
            s -= _b;
            return s;
        }

        protected override double Kernel(Vector<double> xi, Vector<double> xj)
        {
            return xi.DotProduct(xj);
        }
    }

    public abstract class NonLinearSvm : SvmClassifier
    {
        protected override bool IsLinear
        {
            get { return false; }
        }
    }

    public class PolynomialSvm : NonLinearSvm
    {
        protected override double Kernel(Vector<double> xi, Vector<double> xj)
        {
            return Math.Pow(xi * xj, _d);
        }
    }

    public class RbfSvm : NonLinearSvm
    {
        protected override double Kernel(Vector<double> xi, Vector<double> xj)
        {
            return Math.Exp(-.5*Math.Pow((xi - xj).Norm(), 2));
        }
    }
}
