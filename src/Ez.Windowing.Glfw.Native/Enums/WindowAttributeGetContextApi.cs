using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// Used to get window related attributes.
    /// </summary>
    /// <seealso cref="Glfw.GetWindowAttrib(Window*, WindowAttributeGetContextApi)"/>
    public enum WindowAttributeGetContextApi
    {
        /// <summary>
        /// Indicates the context creation API used to create the window's context;
        /// either <see cref="ContextApi.NativeContextApi"/> or <see cref="ContextApi.EglContextApi"/>.
        /// </summary>
        ContextCreationApi = WindowHintContextApi.ContextCreationApi
    }
}
