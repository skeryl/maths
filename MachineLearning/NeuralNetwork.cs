using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures.Matrices;

namespace MachineLearning
{
    public class SupervisedClassifier
    {
    }

    public class UnsupervisedClassifier
    {
    }

    public interface INeuralNetwork
    {
        ICollection<IOutputNode> Outputs { get; set; } 
        
    }

    public abstract class NeuralNetwork : INeuralNetwork
    {
        private Matrix _weights;

        protected NeuralNetwork()
        {
        }

        protected abstract void ConfigureNetwork(Vector<double>[] data, Vector<double>[] targets);

        public void Train(Vector<double>[] data, Vector<double>[] targets)
        {
            if (data.Length != targets.Length)
            {
                throw new ArgumentException("The number of target values must be the same as the number of input values.");
            }
            for (int i = 0; i < targets.Length; i++)
            {
                AddOutputNode();
            }
            ConfigureNetwork(data, targets);

        }

        public ICollection<IOutputNode> Outputs { get; set; }

        protected void AddOutputNode()
        {
            Outputs.Add(new OutputNode());
        }
    }

    public class OutputNode : IOutputNode
    {
        public ICollection<INode> Inputs { get; private set; }

        public OutputNode()
        {
            Inputs = new HashSet<INode>();
        }
    }
    /// <summary>
    /// A Simple neural network with a layer of inputs, a layer of internal nodes, and output nodes. 
    /// </summary>
    public class SimpleNeuralNetwork : NeuralNetwork
    {
        private readonly int _numberInternalNodes;

        public SimpleNeuralNetwork(int numberInternalNodes)
        {
            _numberInternalNodes = numberInternalNodes;
            if (_numberInternalNodes <= 0)
            {
                throw new ArgumentException("Unable to create a neural network with less than 1 internal node.");
            }
            for (int i = 0; i < _numberInternalNodes; i++)
            {
                InternalNodes.Add(new InternalNode());
            }
        }

        public HashSet<IInputNode> InputNodes = new HashSet<IInputNode>();

        public HashSet<IInternalNode> InternalNodes = new HashSet<IInternalNode>();

        protected override void ConfigureNetwork(Vector<double>[] data, Vector<double>[] targets)
        {
            for (int i = 0; i < data.Length; i++)
            {
                InputNodes.Add(new InputNode());
            }

            foreach (var outputNode in Outputs)
            {

            }

        }
    }

    public class InputNode : IInputNode
    {
        public double Value { get; set; }
    }

    public class InternalNode : IInternalNode
    {
        public ICollection<INode> Inputs { get; private set; }

        public InternalNode()
        {
            Inputs = new HashSet<INode>();
        }
    }

    public interface INode
    {
    }

    public interface IInputNode : INode
    {
        double Value { get; set; }
    }

    public interface IInternalNode : INode
    {
        ICollection<INode> Inputs { get; } 
    }

    public interface IOutputNode : INode
    {
        // nodes that input into this output node
        ICollection<INode> Inputs { get; }

        // Add a new input into this output node. Returns true if successful, false if the node already in collection.
        bool AddInput(INode node);
    }
}
