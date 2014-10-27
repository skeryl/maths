using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maths
{
    public interface IOperator
    {
        char Sign { get; }
        int Precedence { get; }
        Association Association { get; }
        int NumberArguments { get; }

        double Evaluate(params double[] args);
    }
}
