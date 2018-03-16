using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recursion
{
    public class Factorial
    {
        public void GetFactorial(int input, ref int result)
        {
            if (result == 0)
            {
                result = input * (input - 1);
                input--;
            }

            if (input > 1)
            {
                result = result * (input - 1);
                input--;
                GetFactorial(input, ref result);
            }
        }

        public int GetFactorial(int input)
        {
            if (input == 1)
            {
                return 1;
            }

            return input * GetFactorial(input - 1);
        }
    }
}
