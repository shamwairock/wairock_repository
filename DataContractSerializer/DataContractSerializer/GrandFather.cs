using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DataContractSerializerDemo
{
    [DataContract]
    [KnownType(typeof(Father))]
    public abstract class GrandFather
    {
        [DataMember]
        public string Name { get; set; }
    }
}
