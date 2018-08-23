using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree
{
    public class Node : INode<int>
    {
        public int Value { get; set; }

        public INode<int> ParentNode { get; set; }

        public INode<int> LeftNode { get; set; }

        public INode<int> RightNode { get; set; }

        public void Insert(int value, INode<int> parentNode)
        {
            if (value == 0 || value == this.Value)
            {
                return;
            }

            if (value < this.Value)
            {
                if (LeftNode == null)
                {
                    LeftNode = new Node {Value = value};
                    this.ParentNode = parentNode;
                }
                else
                {
                    LeftNode.Insert(value, this);
                }
            }
            else
            {
                if (RightNode == null)
                {
                    RightNode = new Node {Value = value};
                    this.ParentNode = parentNode;
                }
                else
                {
                    RightNode.Insert(value, this);
                }
            }
        }

        public void Remove(int value)
        {
            if (value == 0)
            {
                return;
            }

            if (value == this.Value)
            {
                //ToDo
                this.Value = 0;
            }
            else
            {
                LeftNode.Remove(value);
                RightNode.Remove(value);
            }
        }

        public void Traversal()
        {
            throw new NotImplementedException();
        }

        public void PrintTree()
        {
            throw new NotImplementedException();
        }

        public INode<int> BreathFirstSearch(INode<int> node)
        {
            throw new NotImplementedException();
        }

        public INode<int> DepthFirstSearch(INode<int> node)
        {
            throw new NotImplementedException();
        }

        public int GetMin(INode<int> node)
        {
            throw new NotImplementedException();
        }

        public int GetMax(INode<int> node)
        {
            throw new NotImplementedException();
        }
    }
}
