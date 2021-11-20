using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing
{
    /// <summary>
    /// Describes the event of a key.
    /// </summary>
    public enum KeyEvent : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None,
        /// <summary>
        /// Up key event.
        /// </summary>
        Up,
        /// <summary>
        /// Down key event.
        /// </summary>
        Down,
        /// <summary>
        /// Repeat key event.
        /// </summary>
        Repeat
    }
}
