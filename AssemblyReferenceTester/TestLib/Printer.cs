using System;
using System.Reflection;

namespace TestLib
{
    public class Printer
    {
        public string Name { get; set; }

        public void PrintInfo()
        {
            Console.WriteLine("Expansion Printer name is " + Name);
            Console.WriteLine("Printer version is " + Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}
