using Ez.Windowing.GLFW.Native.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native
{
    public class GlfwNativeException : Exception
    {

        public GlfwNativeException(string message, ErrorCode errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ErrorCode ErrorCode { get; }
    }
}
