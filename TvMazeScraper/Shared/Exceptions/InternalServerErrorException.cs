using System;
using System.Runtime.Serialization;

namespace Shared.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException()
        {
        }

        protected InternalServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InternalServerErrorException(string message) : base(message)
        {
        }

        public InternalServerErrorException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}