using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ez.Windowing
{
    /// <summary>
    /// Defines the event data for the <see cref="IWindow.Drop"/> event.
    /// </summary>
    public struct DropEventArgs
    {
        /// <summary>
        /// The drops of event.
        /// </summary>
        public string[] Drops { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DropEventArgs"/> struct.
        /// </summary>
        /// <param name="drops">The string drops in the event.</param>
        public DropEventArgs(string[] drops)
        {
            Drops = drops;
        }
    }
}
