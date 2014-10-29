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
