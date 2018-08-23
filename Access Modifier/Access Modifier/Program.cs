using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access_Modifier_2;

namespace Access_Modifier
{
    class Program
    {
        static void Main(string[] args)
        {
            //we should not be able to access InternalClass2 since it is different namespace
            var internalClass = new InternalClass();
            var internalClass2 = new InternalClass2();
            
        }
    }
}
