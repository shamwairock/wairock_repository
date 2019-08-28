using System;

namespace BitSpread.Security.Exceptions
{
    public class InvalidClosingPriceException : Exception
    {
        public InvalidClosingPriceException(string message)
            : base(message)
        {
        }

        public InvalidClosingPriceException(string message, Exception inner) :
            base(message, inner)
        {
        }
    }
}
