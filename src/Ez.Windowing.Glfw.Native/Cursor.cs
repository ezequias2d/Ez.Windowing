using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native
{
    /// <summary>
    /// Opaque handle to a GLFW cursor.
    /// </summary>
    public struct Cursor
    {
        private IntPtr _handle;

        /// <summary>
        /// Checks if is a null handle.
        /// </summary>
        public bool IsEmpty => _handle == IntPtr.Zero;

        /// <summary>
        /// Cast to IntPtr.
        /// </summary>
        /// <param name="cursor">Cursor</param>
        public static implicit operator IntPtr(Cursor cursor) => cursor._handle;

        /// <summary>
        /// Cast to Cursor
        /// </summary>
        /// <param name="handle">IntPtr</param>
        public static implicit operator Cursor(IntPtr handle) => new() { _handle = handle };
    }
}
