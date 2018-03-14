using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Application_Vs_System_Exception
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TestCustomSystemException();
            }
            catch (CustomSystemException ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                TestCustomApplicationException();
            }
            catch (CustomApplicationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        static void TestCustomSystemException()
        {
            throw new CustomSystemException("System Error.");
        }

        static void TestCustomApplicationException()
        {
            throw new CustomApplicationException("Application Error.");
        }
    }

    [Serializable]
    public class CustomSystemException : SystemException
    {
        public CustomSystemException(string message) : base(message)
        {
        }

        public CustomSystemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class CustomApplicationException : ApplicationException
    {
        public CustomApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CustomApplicationException(string message) : base(message)
        {
        }
    }
}
