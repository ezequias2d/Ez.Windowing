using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// Specifies connected state of devices.
    /// </summary>
    public enum ConnectedState
    {
        /// <summary>
        /// Indicates that a device is connected.
        /// </summary>
        Connected = 0x00040001,

        /// <summary>
        /// Indicates that a device is disconnected.
        /// </summary>
        Disconnected = 0x00040002
    }
}
