using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Var_vs_Dynamic_Variable
{
    class Program
    {
        static void Main(string[] args)
        {
            var obj1 = "obj1 HelloWorld";
            dynamic obj2;

            Console.WriteLine(obj1);

            obj2 = "obj2 HelloWorld2";
            Console.WriteLine(obj2);

            obj2 = 1;
            Console.WriteLine(obj2);

            Console.ReadKey();
        }
    }
}
