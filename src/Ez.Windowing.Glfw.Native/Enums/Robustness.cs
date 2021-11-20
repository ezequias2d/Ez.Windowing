using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// OpenGL context robustness strategy.
    /// </summary>
    public enum Robustness
    {
        /// <summary>
        /// No context robustness strategy.
        /// </summary>
        NoRobustness = 0,

        /// <summary>
        /// Robust context without a reset notification.
        /// </summary>
        NoResetNotification = 0x00031001,

        /// <summary>
        /// Lose context on reset.
        /// </summary>
        LoseContextOnReset = 0x00031002
    }
}
