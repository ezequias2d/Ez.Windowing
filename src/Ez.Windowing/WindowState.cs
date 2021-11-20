using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing
{
    /// <summary>
    /// Describes window state. 
    /// </summary>
    public enum WindowState
    {
        /// <summary>
        /// The window is in its normal state.
        /// </summary>
        Normal,
        /// <summary>
        /// The window covers the whole screen.
        /// </summary>
        FullScreen,
        /// <summary>
        /// The window covers the whole working area.
        /// </summary>
        Maximized,
        /// <summary>
        /// The window is minimized to the taskbar.
        /// </summary>
        Minimized,
        /// <summary>
        /// The window is hidden.
        /// </summary>
        Hidden 
    }
}
