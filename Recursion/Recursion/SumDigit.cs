using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recursion
{
    public class SumDigit
    {
        public int GetSumOfDigit(int input)
        {
            if (input < 10)
            {
                return input;
            }

            var left = GetMostLeftDigit(input);
            var residue = GetResidueDigit(input);

            return GetSumOfDigit(residue) + left;
        }

        private int GetMostLeftDigit(int input)
        {
            return input.ToString().First();
        }

        private int GetResidueDigit(int input)
        {
            var residue = input.ToString().Skip(1).ToArray();
            string s = new string(residue);
            return int.Parse(s);
        }
    }
}
