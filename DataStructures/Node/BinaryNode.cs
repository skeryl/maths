using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Node
{
    public class BinaryNode<T> : INode<T> where T : IComparable
    {
        private readonly T _obj;
        
        public BinaryNode<T> Left { get; set; }
        public BinaryNode<T> Right { get; set; }
                
        public BinaryNode()
        {
            Instances = 0;
        }

        public BinaryNode(T o)
        {
            _obj = o;
            Left = null;
            Right = null;
            Instances = 1;
        }

        public void Increment()
        {
            Instances++;
        }

        public void Add(T obj)
        {
            int compare = this._obj.CompareTo(obj);
            if (compare == 0)
                Instances++;
            else if (compare < 0)
            {
                if(Left == null)
                    Left = new BinaryNode<T>(obj);
                else Left.Add(obj);
            }
            else
            {
                if (Right == null)
                    Right = new BinaryNode<T>(obj);
                else Right.Add(obj);
            }
        }

        public IList<INode<T>> Children { get { return new List<INode<T>> { Left, Right }; } }

        public T Object { get { return _obj; } }
        
        public int Instances { get; protected set; }

        public bool HasChildren { get { return Left != null || Right != null; } }

        public int NumberOfChildren { get { return Children.Count(c => c != null); } }

    }
}
