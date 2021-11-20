using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// Used to get window related attributes.
    /// </summary>
    /// <seealso cref="Glfw.GetWindowAttrib(Window*, WindowAttributeGetRobustness)"/>
    public enum WindowAttributeGetRobustness
    {
        /// <summary>
        /// Indicates the robustness strategy used by the context.
        /// This is <see cref="Robustness.LoseContextOnReset"/> or <see cref="Robustness.NoResetNotification"/>
        /// if the window's context supports robustness, or <see cref="Robustness.NoRobustness"/> otherwise.
        /// </summary>
        ContextRobustness = WindowHintRobustness.ContextRobustness
    }
}
