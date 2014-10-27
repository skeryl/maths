using System;
using System.Collections.Generic;
using System.Globalization;

namespace Maths
{
    public enum Association
    {
        Left,
        Right
    };

    [Serializable]
    public abstract class Operator : IOperator
    {
        public static Dictionary<char, Operator> OperatorSignMap = new Dictionary<char, Operator>
        {
                { '(', new LeftParentheses()},
                { ')', new RightParentheses()},
                { '^', new Exponentiation()},
                { '*', new Multiplication()},
                { '/', new Division()},
                { '+', new Addition()},
                { '-', new Subtraction()}
        };

        public abstract char Sign { get; }
        public abstract int Precedence { get; }
        public abstract Association Association { get; }
        public abstract int NumberArguments { get; }

        public double Evaluate(params double[] args)
        {
            AssertArgsLength(args);
            return DoEvaluation(args);
        }

        protected abstract double DoEvaluation(params double[] args);

        public static bool TryGetOperator (char sign, out Operator op)
        {
            if(OperatorSignMap.ContainsKey(sign))
            {
                op = OperatorSignMap[sign];
                return true;
            }
            op = null;
            return false;
        }

        public override string ToString()
        {
            return Sign.ToString(CultureInfo.InvariantCulture);
        }

        protected void AssertArgsLength(double[] args)
        {
            if (args.Length != NumberArguments)
            {
                throw new ArgumentOutOfRangeException("args", string.Format("The number of arguments required for this operator is: {0}, but {1} were supplied.", NumberArguments, args.Length));
            }
        }

    }

    public class Addition : Operator
    {
        public override char Sign { get { return '+'; } }
        public override int Precedence { get { return 2; } }
        public override Association Association { get { return Association.Left; } }
        public override int NumberArguments { get { return 2; } }

        protected override double DoEvaluation(params double[] args)
        {
            return args[0] + args[1];
        }
    }

    public class Subtraction : Operator
    {
        public override char Sign { get { return '-'; } }
        public override int Precedence { get { return 2; } }
        public override Association Association { get { return Association.Left; } }
        public override int NumberArguments { get { return 2; } }

        protected override double DoEvaluation(params double[] args)
        {
            return args[1] - args[0];
        }
    }

    public class Multiplication : Operator
    {
        public override char Sign { get { return '*'; } }
        public override int Precedence { get { return 3; } }
        public override Association Association { get { return Association.Left; } }
        public override int NumberArguments { get { return 2; } }

        protected override double DoEvaluation(params double[] args)
        {
            return args[1] * args[0];
        }
    }

    public class Division : Operator
    {
        public override char Sign { get { return '/'; } }
        public override int Precedence { get { return 3; } }
        public override Association Association { get { return Association.Left; } }
        public override int NumberArguments { get { return 2; } }

        protected override double DoEvaluation(params double[] args)
        {
            return args[1] / args[0];
        }
    }

    public class Exponentiation : Operator
    {
        public override char Sign { get { return '^'; } }
        public override int Precedence { get { return 4; } }
        public override Association Association { get { return Association.Right; } }
        public override int NumberArguments { get { return 2; } }

        protected override double DoEvaluation(params double[] args)
        {
            return Math.Pow(args[1], args[0]);
        }
    }

    public abstract class Parentheses : Operator
    {
        public override int Precedence { get { return 5; } }
        public override Association Association { get { return Association.Left; } }
        public override int NumberArguments { get { return int.MaxValue; } }

        protected override double DoEvaluation(params double[] args)
        {
            throw new NotImplementedException("Parantheses do not evaluate arguments to any singular value.");
        }
    }

    public class LeftParentheses : Parentheses
    {
        public override char Sign { get { return '('; } }
    }

    public class RightParentheses : Parentheses
    {
        public override char Sign { get { return ')'; } }
    }

    public class Negation : Operator
    {
        public override char Sign { get { return '-'; } }
        public override int Precedence { get { return 3; } }
        public override Association Association { get { return Association.Left; } }
        public override int NumberArguments { get { return 1; } }

        protected override double DoEvaluation(params double[] args)
        {
            return -args[0];
        }
        
    }
}
