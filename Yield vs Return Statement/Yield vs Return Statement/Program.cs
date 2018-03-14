using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Yield_vs_Return_Statement
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(SimpleReturn());
            Console.WriteLine(SimpleReturn());
            Console.WriteLine(SimpleReturn());

            YieldReturn().ToList().ForEach(Console.WriteLine);

            Console.ReadKey();
        }

        static int SimpleReturn()
        {
            return 1;
            return 2;
            return 3;
        }

        static IEnumerable<int> YieldReturn()
        {
            Console.WriteLine("Step 1");
            yield return 1;

            Console.WriteLine("Step 2");
            yield return 2;

            Console.WriteLine("Step 3");
            yield return 3;

            Console.WriteLine("Step 4");
        }
    }
}
