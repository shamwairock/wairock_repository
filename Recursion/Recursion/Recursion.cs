using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recursion
{
    public static class Recursion
    {
        public static uint GetFactorialNumber(uint input)
        {
            if (input == 1)
            {
                return 1;
            }

            return GetFactorialNumber(input - 1) * input;
        }

        public static uint GetSumOfDigit(uint input)
        {
            if (input < 10)
            {
                return input;
            }
            
            
            var leftDigit = Convert.ToUInt32(input.ToString().First().ToString());
            var residue = Convert.ToUInt32(new string(input.ToString().Skip(1).ToArray()));

            return leftDigit + GetSumOfDigit(residue);
        }

        public static string ConvertIntToBinaryFormat(uint input)
        {
            var result = ConvertIntToBinary(input);
           return new string(result.ToCharArray().Reverse().ToArray());
        }

        private static string ConvertIntToBinary(uint input)
        {
            if (input == 1)
            {
                return input.ToString();
            }

            var dividend = input;
            var divisor = 2;

            var quotient = (uint)(dividend/divisor);
            var remainder = dividend%divisor;

            return remainder + ConvertIntToBinary(quotient);
        }

        public static int PowerOfNumber(int baseNumber, int power)
        {
            if (power == 1)
            {
                return baseNumber;
            }

            return baseNumber* PowerOfNumber(baseNumber, power - 1);
        }

        public static LinkedList<string> ReverseLinkedList(LinkedList<string> input)
        {
            if (input.Count == 0)
            {
                return input;
            }

            var firstValue = input.First.Value;
            input.RemoveFirst();
            ReverseLinkedList(input).AddLast(firstValue);

            return input;
        }
    }
}
