using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Difference_Func_and_Action
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<int> myAction = DoSomething;
            myAction(123);           // Prints out "123"
                                     // can be also called as myAction.Invoke(123);

            Func<int, int, double> myFunc = new Func<int, int, double>(CalculateSomething);
            Console.WriteLine(myFunc(5,2));   // Prints out "2.5"

            Console.ReadKey();
        }

        static void DoSomething(int i)
        {
            Console.WriteLine(i);
        }

        static double CalculateSomething(int i, int j)
        {
            try
            {
                return (double) i/j;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
