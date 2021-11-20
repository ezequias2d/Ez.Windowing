using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// Used to get window related attributes.
    /// </summary>
    /// <seealso cref="Glfw.GetWindowAttrib(Window*, WindowAttributeGetInt)"/>
    public enum WindowAttributeGetInt
    {
        /// <summary>
        /// Indicate the client API version(major part) of the window's context.
        /// </summary>
        ContextVersionMajor = WindowHintInt.ContextVersionMajor,

        /// <summary>
        /// Indicate the client API version(minor part) of the window's context.
        /// </summary>
        ContextVersionMinor = WindowHintInt.ContextVersionMinor,

        /// <summary>
        /// Indicate the client API version(revision part) of the window's context.
        /// </summary>
        ContextVersionRevision = WindowHintInt.ContextRevision
    }
}
