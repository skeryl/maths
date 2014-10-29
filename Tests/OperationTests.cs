using System;
using Maths;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class OperationTests
    {
        [Test]
        public void TestBasicEvaluation()
        {
            var math = new Evaluator();

            const string expression1 = "(3+7)*10/2";
            Assert.AreEqual(math.EvaluateExpression(expression1), 50);

            const string expression2 = "3+7*10/2";
            Assert.AreEqual(math.EvaluateExpression(expression2), 38);

            const string expression3 = "((((2.5*2^2)/5)^2)+3)*2";
            Assert.AreEqual(math.EvaluateExpression(expression3), 14);
        }

        [Test]
        public void TestVariableEvaluation()
        {
            var math = new Evaluator();

            const string expression1 = "(2*x^2) + (3*y) + z";

            var value1 = math.EvaluateExpression(expression1, new { x = 2, y = 10, z = 2 });
            Assert.AreEqual(value1, 40);

            var value2 = math.EvaluateExpression(expression1, new { x = 1, y = 2, z = 2 });
            Assert.AreEqual(value2, 10);

            const string expression2 = "((variableOne*2+3)/variableTwo)+variableOne";

            var value3 = math.EvaluateExpression(expression2, new { variableOne = 2, variableTwo = 10 });
            Assert.AreEqual(value3, 2.7);

        }
    }
}
