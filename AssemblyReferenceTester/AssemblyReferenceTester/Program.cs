using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TestLib;

namespace AssemblyReferenceTester
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var printer = new Printer {Name = "FujiXerox"};
                printer.PrintInfo();

                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
