using DataStructures.Node;

namespace DataStructures.Tree
{
    public interface ITree<T>
    {
        INode<T> RootNode { get; }
        INode<T> Find(T obj);
        void Insert(T obj);
        bool Contains(T obj);
    }
}
