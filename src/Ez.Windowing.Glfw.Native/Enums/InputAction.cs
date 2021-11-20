﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// Defines event information for <see cref="GlfwCallbacks.KeyCallback"/>
    /// or <see cref="GlfwCallbacks.MouseButtonCallback"/>.
    /// </summary>
    public enum InputAction
    {
        /// <summary>
        /// The key or mouse button was released.
        /// </summary>
        Release = 0,

        /// <summary>
        /// The key or mouse button was pressed.
        /// </summary>
        Press = 1,

        /// <summary>
        /// The key was held down until it repeated.
        /// </summary>
        Repeat = 2
    }
}
