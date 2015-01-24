using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class SupervisedClassifier
    {
    }

    public class UnsupervisedClassifier
    {
    }

    public class NeuralNetwork
    {

    }

    public interface INode
    {
    }

    public interface IInputNode : INode
    {
        double Value { get; set; }
    }

    public interface IInternalNode : INode {}

    public interface IOutputNode : INode
    {
    }
}
