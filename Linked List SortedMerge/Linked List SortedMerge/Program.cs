using System;
using System.Collections.Generic;

namespace Linked_List_SortedMerge
{
    class Program
    {
        static void Main(string[] args)
        {
            var list1 = new LinkedList<int>();
            list1.AddLast(1);
            list1.AddLast(2);
            list1.AddLast(3);
            list1.AddLast(4);
            list1.AddLast(5);
            list1.AddLast(9999);
            list1.AddLast(100000);
            list1.AddLast(22323232);

            var list2 = new LinkedList<int>();
            list2.AddLast(2);
            list2.AddLast(4);
            list2.AddLast(8);

            var ihead = list1.First;
            var jhead = list2.First;

            while (list2.Count != 0)
            {
                for(int i=0;i< list1.Count; i++)
                {
                    if (jhead.Value <= ihead.Value)
                    {
                        ihead.List.AddBefore(ihead, jhead.Value);
                        break;
                    }
                    else
                    {
                        ihead = ihead.Next;
                        continue;
                    }
                }

                if (list2.Count == 0)
                {
                    break;
                }

                list2.RemoveFirst();
                jhead = list2.First;
            }

            foreach (var node in list1)
            {
                Console.Write(node + " ");
            }

            Console.Read();

        }
    }
}
