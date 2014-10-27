using System.Collections.Generic;

namespace DataStructures.Node
{
    public class Node<T> : INode<T>
    {
        private readonly T _obj;
        
        public Node()
        {
            Instances = 0;
            Children = new List<INode<T>>();
        }

        public Node(T o)
        {
            _obj = o;
            Instances = 1;
            Children = new List<INode<T>>();
        }

        public void Increment()
        {
            Instances++;
        }

        public void Add(T obj)
        {
            Children.Add(new Node<T>(obj));
        }

        public IList<INode<T>> Children { get; protected set; } 

        public T Object { get { return _obj; } }
        
        public int Instances { get; protected set; }

        public int NumberOfChildren { get { return Children == null? 0 : Children.Count; } }

        public bool HasChildren
        {
            get { return NumberOfChildren > 0; }
        }
    }
}
