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
            Factorial factorial = new Factorial();
            int result = 0;
            factorial.GetFactorial(5, ref result);
            Console.WriteLine(result);
            Console.WriteLine(factorial.GetFactorial(5));


            SumDigit sumDigit = new SumDigit();
            Console.WriteLine(sumDigit.GetSumOfDigit(999));

            Console.ReadKey();
        }
    }
}
