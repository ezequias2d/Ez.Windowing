using System;

namespace Ez.Windowing.GLFW.Native
{
    /// <summary>
    /// Opaque handle to a GLFW window.
    /// </summary>
    public struct Window
    {
        private IntPtr _handle;

        /// <summary>
        /// Check if is null handle.
        /// </summary>
        public bool IsEmpty => _handle == IntPtr.Zero;
        
        /// <summary>
        /// Cast to IntPtr
        /// </summary>
        /// <param name="window">Window</param>
        public static implicit operator IntPtr(Window window) => window._handle;

        /// <summary>
        /// Cast to Window
        /// </summary>
        /// <param name="handle">Handle</param>
        public static implicit operator Window(IntPtr handle) => new() { _handle = handle };
    }
}
