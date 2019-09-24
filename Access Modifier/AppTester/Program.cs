using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Access_Modifier;
using Newtonsoft.Json;

namespace AppTester
{
    class Program
    {
        static void Main(string[] args)
        {
            double doubleValInfo = double.PositiveInfinity;
            var internalClass = new InternalClass(){Name="wai rock", Value = doubleValInfo };

            var json = new JavaScriptSerializer().Serialize(internalClass);
            Console.WriteLine(json);

            var newton = JsonConvert.SerializeObject(internalClass);
            Console.WriteLine(newton);

            Console.Read();
        }
    }
}
