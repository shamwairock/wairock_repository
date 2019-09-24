using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access_Modifier
{
    public class ChildClass : ProtectedBaseClass
    {
        public string GetString(byte[] buffer)
        {
            return GetHexString(buffer);
        }
    }
}
