using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// Attribute for setting <see cref="CursorModeValue"/> of the cursor.
    /// </summary>
    /// <seealso cref="Glfw.SetInputMode(Window,CursorStateAttribute,CursorModeValue)"/>
    /// <seealso cref="Glfw.GetInputMode(Window,CursorStateAttribute)"/>
    public enum CursorStateAttribute
    {
        /// <summary>
        /// Attribute for setting <see cref="CursorModeValue"/> of the cursor.
        /// </summary>
        Cursor = 0x00033001,
    }
}
