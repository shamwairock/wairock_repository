using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree
{
    public interface INode<T>
    {
        T Value {get;set;}

        INode<T> ParentNode { get; set; }

        INode<T> LeftNode { get; set; }

        INode<T> RightNode { get; set; }
        
        /// <summary>
        /// Insertion is always insert to the left if the input is smaller than the root.
        /// If node is duplicated, do not insert.
        /// </summary>
        /// <param name="node"></param>
        void Insert(T value, INode<int> parentNode);

        void Remove(T value);

        void Traversal();

        void PrintTree();

        INode<T> BreathFirstSearch(INode<T> node);

        INode<T> DepthFirstSearch(INode<T> node);

        /// <summary>
        /// Returns the data in the furthest left node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        T GetMin(INode<T> node);

        /// <summary>
        /// Return the data in the furthest right node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        T GetMax(INode<T> node);
    }
}
