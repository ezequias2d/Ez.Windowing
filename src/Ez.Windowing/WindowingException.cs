using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Ez.Windowing
{
    /// <inheritdoc/>
    public class WindowingException : Exception
    {
        /// <inheritdoc/>
        public WindowingException()
        {
        }

        /// <inheritdoc/>
        public WindowingException(string message) : base(message)
        {
        }

        /// <inheritdoc/>
        public WindowingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <inheritdoc/>
        protected WindowingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
