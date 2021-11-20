using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// Used to get window related attributes.
    /// </summary>
    /// <seealso cref="Glfw.GetWindowAttrib(Window*, WindowAttributeGetClientApi)"/>
    public enum WindowAttributeGetClientApi
    {
        /// <summary>
        /// Indicates the client API provided by the window's context;
        /// either <see cref="GraphicsLibraryFramework.ClientApi.OpenGlApi"/>,
        /// <see cref="GraphicsLibraryFramework.ClientApi.OpenGlEsApi"/> or
        /// <see cref="GraphicsLibraryFramework.ClientApi.NoApi"/>.
        /// </summary>
        ClientApi = WindowHintClientApi.ClientApi
    }
}
