using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitWise_Operator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Bitwise AND a & b   Returns a one in each bit position for which the corresponding bits of both operands are ones.
            //Bitwise OR  a | b   Returns a zero in each bit position for which the corresponding bits of both operands are zeros.
            //Bitwise XOR a ^ b   Returns a zero in each bit position for which the corresponding bits are the same.
            //[Returns a one in each bit position for which the corresponding bits are different.]
            //Bitwise NOT ~a Inverts the bits of its operand.

            var x = 16;
            var y = 24;

            var bitwiseAND = x & y;
            var bitwiseOR = x | y;
            var bitwiseXOR = x ^ y;

            Console.WriteLine("Where x = 16, y = 24");
            Console.WriteLine("bitwiseAND = x & y (16) ------- " + bitwiseAND);
            Console.WriteLine("bitwiseOR  = x | y (24) ------- " + bitwiseOR);
            Console.WriteLine("bitwiseXOR = x ^ y (8)  ------- " + bitwiseXOR);
           
            Console.Read();
        }
    }
}
