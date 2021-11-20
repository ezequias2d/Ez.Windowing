using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing
{
    /// <summary>
    /// Key modifiers, such as Shift or CTRL.
    /// </summary>
    [Flags]
    public enum KeyModifiers
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,
        /// <summary>
        /// if one or more Shift keys were held down.
        /// </summary>
        Shift = 1,

        /// <summary>
        /// If one or more Control keys were held down.
        /// </summary>
        Control = 1 << 2,

        /// <summary>
        /// If one or more Alt keys were held down.
        /// </summary>
        Alt = 1 << 3,

        /// <summary>
        /// If one or more Super keys were held down.
        /// </summary>
        Super = 1 << 4,

        /// <summary>
        ///     If caps lock is enabled.
        /// </summary>
        CapsLock = 1 << 5,

        /// <summary>
        ///     If num lock is enabled.
        /// </summary>
        NumLock = 1 << 6,
    }
}
