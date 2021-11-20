using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// The context client APIs.
    /// </summary>
    public enum ClientAPI
    {
        /// <summary>
        /// No context API is created.
        /// </summary>
        NoAPI = 0,

        /// <summary>
        /// OpenGL context is created.
        /// </summary>
        OpenGL = 0x00030001,

        /// <summary>
        /// OpenGL ES context is created.
        /// </summary>
        OpenGLES = 0x00030002
    }
}
