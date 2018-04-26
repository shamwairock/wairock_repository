using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recursion
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Recursion.GetSumOfDigit(999));
            Console.WriteLine(Recursion.GetFactorialNumber(5));
            Console.WriteLine(Recursion.ConvertIntToBinaryFormat(599));
            Console.WriteLine(Recursion.PowerOfNumber(2,5));

            var linkedList = new LinkedList<string>();
            linkedList.AddFirst("you");
            linkedList.AddLast("change");
            linkedList.AddLast("nothing");
            linkedList.AddLast("1");
            linkedList.AddLast("2");
            linkedList.AddLast("3");

            foreach (var s in linkedList)
            {
                Console.Write(s + " ");
            }
            Console.WriteLine();
           
            foreach (var s in Recursion.ReverseLinkedList(linkedList))
            {
                Console.Write(s + " ");
            }
            Console.WriteLine();

            Console.Write("Fibonacci result: ");
            foreach (var s in Recursion.GetFibonacciSeries(8))
            {
                Console.Write(s + " ");
            }
            Console.WriteLine();

            var dictionary = new Dictionary<uint, string>
            {
                {8, "eight"},
                {7, "seven"},
                {6, "six"},
                {5, "five"},
                {1, "one"},
                {2, "two"},
                {3, "three"},
                {4, "four"}
            };

            Console.Write("Binary search for key 8:" + Recursion.BinarySearch(8, dictionary));
            Console.WriteLine();
            Console.Write("Binary search for key 4:" + Recursion.BinarySearch(4, dictionary));
            Console.WriteLine();
            Console.Write("Binary search for key 99:" + Recursion.BinarySearch(99, dictionary));

            Console.ReadKey();
        }
    }
}
