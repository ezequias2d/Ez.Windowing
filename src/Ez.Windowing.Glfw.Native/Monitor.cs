using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native
{
    /// <summary>
    /// Opaque handle to a GLFW monitor.
    /// </summary>
    public struct Monitor
    {
        private IntPtr _handle;

        public bool IsEmpty => _handle == IntPtr.Zero;

        public static implicit operator IntPtr(Monitor monitor) => monitor._handle;
        public static implicit operator Monitor(IntPtr handle) => new Monitor { _handle = handle };

        public static Monitor Empty => new() { _handle = IntPtr.Zero };
    }
}
