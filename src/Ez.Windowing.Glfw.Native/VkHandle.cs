using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
namespace Ez.Windowing.GLFW.Native
{
    /// <summary>
    ///     A handle to a Vulkan object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VkHandle
    {
        private IntPtr Handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="VkHandle"/> struct.
        /// </summary>
        /// <param name="handle">
        /// The native Vulkan handle.
        /// This is NOT a pointer to a field containing the handle, this is the actual handle itself.
        /// </param>
        public VkHandle(IntPtr handle)
        {
            Handle = handle;
        }

        public static implicit operator IntPtr(VkHandle vk) =>
            vk.Handle;
        public static explicit operator VkHandle(IntPtr handle) =>
            new VkHandle { Handle = handle };
    }
}
