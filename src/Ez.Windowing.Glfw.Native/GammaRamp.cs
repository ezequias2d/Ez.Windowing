using Ez.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native
{
    /// <summary>
    /// Gamma ramp for a <see cref="MonitorHandle"/>.
    /// </summary>
    public struct GammaRamp
    {
        private IntPtr _red;
        private IntPtr _green;
        private IntPtr _blue;
        private uint _size;

        /// <summary>
        /// Red components of the gamma ramp.
        /// </summary>
        public ReadOnlySpan<ushort> Red => MemUtil.GetSpan<ushort>(_red, (int)_size);

        /// <summary>
        /// Green components of the gamma ramp.
        /// </summary>
        public ReadOnlySpan<ushort> Green => MemUtil.GetSpan<ushort>(_green, (int)_size);

        /// <summary>
        /// Blue components of the gamma ramp.
        /// </summary>
        public ReadOnlySpan<ushort> Blue => MemUtil.GetSpan<ushort>(_blue, (int)_size);
    }
}
