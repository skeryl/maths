using System.Collections.Generic;

namespace DataStructures.Node
{
    public interface INode<T>
    {
        T Object { get; }
        int Instances { get; }
        bool HasChildren { get; }
        IList<INode<T>> Children { get; }
        int NumberOfChildren { get; }
        void Increment();
        void Add(T obj);
    }
}
