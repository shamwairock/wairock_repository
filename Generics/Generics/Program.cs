using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generics
{
    class Program
    {
        static void Main(string[] args)
        {
            var stack = new OrdinaryStack();
            var tStack = new GenericStack<int>();

            try
            {
                stack.Push("hello");
                Console.WriteLine("Push:hello");

                stack.Push("world");
                Console.WriteLine("Push:world");

                Console.WriteLine("Peek:" + stack.Peek());
                Console.WriteLine("Pop:" + stack.Pop());
                Console.WriteLine("Pop:" + stack.Pop());

                tStack.Push(1);
                Console.WriteLine("Push:1");

                tStack.Push(2);
                Console.WriteLine("Push:2");

                Console.WriteLine("Peek:" + tStack.Peek());
                Console.WriteLine("Pop:" + tStack.Pop());
                Console.WriteLine("Pop:" + tStack.Pop());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}
