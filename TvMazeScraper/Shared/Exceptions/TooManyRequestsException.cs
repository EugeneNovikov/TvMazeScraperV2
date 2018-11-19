using System;
using System.Runtime.Serialization;

namespace Shared.Exceptions
{
    public class TooManyRequestsException : Exception
    {
        public TooManyRequestsException()
        {
        }

        protected TooManyRequestsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TooManyRequestsException(string message) : base(message)
        {
        }

        public TooManyRequestsException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}