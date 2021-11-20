using Ez.Windowing.GLFW.Native.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Ez.Windowing.GLFW
{
    /// <inheritdoc/>
    public class GlfwException : WindowingException
    {
        /// <inheritdoc/>
        public GlfwException()
        {
        }

        /// <inheritdoc/>
        public GlfwException(string message) : base(message)
        {

        }

        /// <inheritdoc/>
        public GlfwException(string message, Exception innerException) : base(message, innerException)
        {

        }

        /// <inheritdoc/>
        protected GlfwException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
