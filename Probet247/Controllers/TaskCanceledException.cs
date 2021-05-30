using System;
using System.Runtime.Serialization;

namespace Probet247.Controllers
{
    [Serializable]
    internal class TaskCanceledException : Exception
    {
        public TaskCanceledException()
        {
        }

        public TaskCanceledException(string message) : base(message)
        {
        }

        public TaskCanceledException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TaskCanceledException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}