using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContractSerializerDemo
{
   
    [DataContract]
    public class Father : GrandFather
    {
        [DataMember]
        public string Age { get; set; }
    }
}
