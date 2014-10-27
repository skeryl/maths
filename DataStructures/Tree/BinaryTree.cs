using System;
using DataStructures.Node;

namespace DataStructures.Tree
{
    public class BinaryTree<T> : ITree<T> where T : IComparable
    {
        public INode<T> RootNode { get; protected set; }

        public BinaryTree()
        {
        }

        public BinaryTree(T rootObject)
        {
            RootNode = new BinaryNode<T>(rootObject);
        }

        public INode<T> Find(T obj)
        {
            return Find(obj, (BinaryNode<T>) RootNode);
        }

        private INode<T> Find(T obj, BinaryNode<T> node)
        {
            if(node == null)
                return null;

            int compare = node.Object.CompareTo(obj);

            if (compare == 0)
                return node;

            if (compare < 0)
                return Find(obj, node.Left);
            
            return Find(obj, node.Right);
        }

        public void Insert(T obj)
        {
            if(RootNode == null)
                RootNode = new BinaryNode<T>(obj);
            else
                RootNode.Add(obj);
        }

        public bool Contains(T obj)
        {
            return Find(obj) != null;
        }
    }
}
