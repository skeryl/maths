using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Maths;
using NUnit.Framework;
using Newtonsoft.Json;

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

            var value1_2 = math.EvaluateExpression(expression1, new Dictionary<string, object> { { "x", 2 }, { "y", 10 }, { "z", 2 } });
            Assert.AreEqual(value1_2, 40);

            var value2 = math.EvaluateExpression(expression1, new { x = 1, y = 2, z = 2 });
            Assert.AreEqual(value2, 10);

            const string expression2 = "((variableOne*2+3)/variableTwo)+variableOne";

            var value3 = math.EvaluateExpression(expression2, new { variableOne = 2, variableTwo = 10 });
            Assert.AreEqual(value3, 2.7);
        }

        [Test]
        public void TestEvaluateFromExpression()
        {
            var math = new Evaluator();

            const string expression1 = "(2*x^2) + (3*y) + z";

            Expression expression = math.Parse(expression1);
            Assert.AreEqual(expression.Evaluate(new { x = 2, y = 10, z = 2 }), 40);
            Assert.AreEqual(expression.Evaluate(new { x = 3, y = 10, z = 2 }), 50);

            var strExpression = expression.ToString();
            Console.WriteLine(strExpression);
            Assert.IsTrue(strExpression.Contains(expression1));

        }

        [Test][Ignore("This isn't supported yet.")]
        public void TestSerializeJson()
        {
            // todo: make this Json Serializable
           /* Expression expression = new Evaluator().Parse("(2*x^2) + (3*y) + z");
            var json = ToJson(expression);
            var deserialized = FromJson<Expression>(json);
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(expression.ToString(), deserialized.ToString()); */
        }

        [Test][Ignore("This isn't supported yet.")]
        public void TestXmlSerialization()
        {
            // todo: make this Xml Serializable
            /*Expression expression = new Evaluator().Parse("(2*x^2) + (3*y) + z");
            var xml = ToXml(expression);
            var deserialized = FromXml<Expression>(xml);
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(expression.ToString(), deserialized.ToString());*/
        }

        private string ToJson<T>(T obj)
        {
            return ((object)obj == null) ? string.Empty : JsonConvert.SerializeObject(obj);
        }

        private T FromJson<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        private string ToXml<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            var stringBuilder = new StringBuilder();
            using(var textWriter = new StringWriter(stringBuilder))
            {
                serializer.Serialize(textWriter, obj);
            }
            return stringBuilder.ToString();
        }

        private T FromXml<T>(string str) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            T obj;
            using(var textReader = new StringReader(str))
            {
                obj = serializer.Deserialize(textReader) as T;
            }
            return obj;
        }

    }
}
