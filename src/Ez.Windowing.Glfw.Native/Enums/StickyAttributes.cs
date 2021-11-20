using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// Attributes related to sticky keys and buttons.
    /// </summary>
    /// <seealso cref="Glfw.SetInputMode(Window*,StickyAttributes,bool)"/>
    /// <seealso cref="Glfw.GetInputMode(Window*,StickyAttributes)"/>
    public enum StickyAttributes
    {
        /// <summary>
        /// Specify whether keyboard input should be sticky or not.
        /// </summary>
        StickyKeys = 0x00033002,

        /// <summary>
        /// Specify whether mouse button input should be sticky or not.
        /// </summary>
        StickyMouseButton = 0x00033003
    }
}
