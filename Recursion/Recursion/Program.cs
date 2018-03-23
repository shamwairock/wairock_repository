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

            Console.ReadKey();
        }
    }
}
