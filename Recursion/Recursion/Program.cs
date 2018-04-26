using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recursion
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Recursion.GetFactorial(5));

            Console.WriteLine(Recursion.GetSumOfDigits(10));

            Console.WriteLine(Recursion.IntToBinary(8));

            Console.ReadKey();
        }
    }
}
