using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// The context API used to create the window context.
    /// </summary>
    public enum ContextAPI
    {
        /// <summary>
        /// Uses the native context API to create the window context.
        /// </summary>
        NativeContextApi = 0x00036001,

        /// <summary>
        /// Uses Egl to create the window context.
        /// </summary>
        EglContextApi = 0x00036002
    }
}
