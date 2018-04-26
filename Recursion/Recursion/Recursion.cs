using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recursion
{
    public static class Recursion
    {
        public static int GetFactorial(int input)
        {
            if(input == 1)
            {
                return input;
            }

            return input * GetFactorial(input - 1);
        }

        public static uint GetSumOfDigits(uint input)
        {
            if(input < 10)
            {
                return input;
            }

            var leftDigit = uint.Parse(input.ToString().First().ToString());
            var residue = uint.Parse(new string(input.ToString().Skip(1).ToArray()));

            return GetSumOfDigits(residue) + leftDigit;
        }

        public static string IntToBinary(int input)
        {
            return Convert.ToString(input, 2);
        }
    }
}
