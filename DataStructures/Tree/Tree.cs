using System;
using System.Linq;
using DataStructures.Node;

namespace DataStructures.Tree
{
    public class Tree<T> : ITree<T>
    {
        private const int LEAF_COUNT = 3;

        private readonly Random _random = new Random();

        public INode<T> RootNode { get; protected set; }

        public Tree()
        {
        }

        public Tree(T rootObject)
        {
            RootNode = new Node<T>(rootObject);
        }

        public INode<T> Find(T obj)
        {
            return Find(obj, RootNode);
        }

        private INode<T> Find(T obj, INode<T> node)
        {
            INode<T> result = null;
            if (obj.Equals(node.Object))
                result = node;
            else
                foreach (INode<T> child in node.Children)
                    if (result == null)
                        result = Find(obj, child);
            return result;
        }

        public void Insert(T obj)
        {
            if(RootNode == null)
                RootNode = new Node<T>(obj);
            else
                Insert(obj, RootNode);
        }

        private void Insert(T obj, INode<T> node)
        {
            if(node.NumberOfChildren <= LEAF_COUNT)
                node.Add(obj);
            else
            {
                Insert(obj, node.Children.ElementAt(_random.Next(0, node.NumberOfChildren)));
            }
        }

        public bool Contains(T obj)
        {
            return Find(obj) != null;
        }
    }
}
