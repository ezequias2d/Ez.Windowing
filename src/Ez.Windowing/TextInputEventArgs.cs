using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing
{
    /// <summary>
    ///     Raised when a single unicode code point is input.
    /// </summary>
    public readonly struct TextInputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputEventArgs"/> struct.
        /// </summary>
        /// <param name="unicode">The unicode code point that was input.</param>
        public TextInputEventArgs(int unicode)
        {
            Unicode = unicode;
        }

        /// <summary>
        ///     Gets the Unicode code point that was input.
        /// </summary>
        public int Unicode { get; }

        /// <summary>
        ///     Gets the string representation of the input Unicode code point.
        /// </summary>
        public string GetString() => char.ConvertFromUtf32(Unicode);
    }
}
