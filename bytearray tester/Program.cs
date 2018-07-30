using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bytearray_tester
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] byte1 = new byte[4] { 97, 41, 203, 59 };
            byte[] byte2 = new byte[4] { 95, 41, 203, 59 };

            try
            {
                var test1 = BitConverter.ToDouble(byte1, 0);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
