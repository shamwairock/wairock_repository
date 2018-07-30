using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContractSerializerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Father father = new Father();
            father.Name = "Sham Kooi Yeong";
            father.Age = "45";

            using (FileStream writer = new FileStream(@"DataContractSerializerExample.xml", FileMode.Create))
            {
                var serializer = new DataContractSerializer(typeof(Father));
                serializer.WriteObject(writer, father);
            }
        }
    }
}
