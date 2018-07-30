using System;
using System.Collections.Generic;

namespace Linked_List_Reversal
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkedList<string> myLinkedList = new LinkedList<string>();
            myLinkedList.AddLast("1");
            myLinkedList.AddLast("2");
            myLinkedList.AddLast("3");
            myLinkedList.AddLast("4");
            myLinkedList.AddLast("5");
            myLinkedList.AddLast("6");
            myLinkedList.AddLast("7");
            myLinkedList.AddLast("8");
            myLinkedList.AddLast("9");
            myLinkedList.AddLast("10");
            myLinkedList.AddLast("11");
            myLinkedList.AddLast("12");


            foreach (var node in myLinkedList)
            {
                Console.Write(node + " ");
            }

            Console.WriteLine();

            var head = myLinkedList.First;
            var tail = myLinkedList.Last;

            //Reverse
            for (int i = 0; i < myLinkedList.Count - 1 ; i++)
            {
                if (i == 0)
                {
                    head.List.AddFirst(head.List.Last.Value);
                    head = myLinkedList.First;
                }
                else
                {
                    myLinkedList.AddAfter(head, head.List.Last.Value);
                    head = head.Next;
                }

                myLinkedList.RemoveLast();

            }


            foreach (var node in myLinkedList)
            {
                Console.Write(node + " ");
            }

            Console.WriteLine();

            Console.Read();
        }
    }
}
