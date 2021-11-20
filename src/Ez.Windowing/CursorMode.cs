using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing
{
    /// <summary>
    /// The cursor modes.
    /// </summary>
    public enum CursorMode
    {
        /// <summary>
        /// Hides the cursor when over the <see cref="IWindow"/>.
        /// </summary>
        Hidden,
        /// <summary>
        /// Shows the cursor when over the <see cref="IWindow"/>.
        /// </summary>
        Visible,
        /// <summary>
        /// Hides the cursor and lock it to a <see cref="IWindow"/>.
        /// The input readings and movement of the mouse must be recognized normally.
        /// </summary>
        Disabled
    }
}
