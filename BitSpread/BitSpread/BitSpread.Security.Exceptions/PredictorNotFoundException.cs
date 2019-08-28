using System;

namespace BitSpread.Security.Exceptions
{
    public class PredictorNotFoundException : Exception
    {
        public PredictorNotFoundException(string message)
            : base(message)
        {
        }

        public PredictorNotFoundException(string message, Exception inner) :
            base(message, inner)
        {
        }
    }
}
