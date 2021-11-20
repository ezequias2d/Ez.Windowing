using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing
{
    /// <summary>
    /// Describes window borders.
    /// </summary>
    public enum WindowBorder
    {
        /// <summary>
        /// The window does not have a border.
        /// </summary>
        Hidden,
        /// <summary>
        /// The window has a resizable border.
        /// </summary>
        Resizable,
        /// <summary>
        /// The window has a fixed border.
        /// </summary>
        Fixed
    }
}
