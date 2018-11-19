using System;
using System.Runtime.Serialization;

namespace Shared.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }


        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
