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

        public static uint[] GetFibonacciSeries(uint input, uint[] series = null)
        {
            uint f1;
            uint f2;
           
            if (series == null)
            {
                series = new uint[3];

                f1 = 1;
                f2 = 1;

                series[0] = f1;
                series[1] = f2;
                series[2] = f1 + f2;
                
                return GetFibonacciSeries(input, series);
            }
            else
            {
                if (series.Length == input)
                {
                    return series;
                }
                else
                {
                    f1 = series[series.Length - 1];
                    f2 = series[series.Length - 2];

                    Array.Resize(ref series, series.Length + 1);
                    series[series.Length - 1] = f1 + f2;

                    return GetFibonacciSeries(input, series);
                }
            }
        } 
    }
}
