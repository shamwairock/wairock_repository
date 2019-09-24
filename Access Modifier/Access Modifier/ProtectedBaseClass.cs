using System;

namespace Access_Modifier
{
    public class ProtectedBaseClass
    {
        protected static string GetHexString(byte[] buffer)
        {
            return BitConverter.ToString(buffer);
        }
    }
}
