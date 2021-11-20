using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// Attribute for setting caps and num lock bits on when setting the input mode.
    /// </summary>
    /// <seealso cref="Glfw.SetInputMode(Window, LockKeyModAttribute, bool)"/>
    /// <seealso cref="Glfw.GetInputMode(Window, LockKeyModAttribute)"/>
    public enum LockKeyModAttribute
    {
        /// <summary>
        /// Specify whether the lock key bits should be set or not.
        /// </summary>
        LockKeyMods = 0x00033004
    }
}
