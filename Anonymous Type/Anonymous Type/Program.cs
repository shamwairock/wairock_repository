using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymous_Type
{
    class Program
    {
        static void Main(string[] args)
        {
            var anonymous = new { Price = 10, Tag = "Product1" };
            Console.WriteLine("Price " + anonymous.Price);
            Console.WriteLine("Tag " + anonymous.Tag);

            Console.Read();
        }
    }
}
