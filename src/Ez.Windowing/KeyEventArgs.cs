using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing
{
    /// <summary>
    /// Defines the event data for the <see cref="IWindow.KeyDown"/>, 
    /// <see cref="IWindow.KeyRepeat"/>, or <see cref="IWindow.KeyUp"/>.
    /// </summary>
    public readonly struct KeyEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyEventArgs"/> struct.
        /// </summary>
        /// <param name="key">The <see cref="Key"/> that generated this event.</param>
        /// <param name="modifiers">The <see cref="KeyModifiers"/> that generated this event.</param>
        /// <param name="scanCode">The keyboard scan code of the key that generated this event.</param>
        /// <param name="keyEvent">The <see cref="Event"/> that generated this event.</param>
        public KeyEventArgs(Key key, KeyModifiers modifiers, int scanCode, KeyEvent keyEvent)
        {
            (Key, Modifiers, ScanCode, Event) = 
                (key, modifiers, scanCode, keyEvent);
        }

        /// <summary>
        /// Gets the <see cref="Key"/> that generated this event.
        /// </summary>
        public Key Key { get; }

        /// <summary>
        /// Gets the <see cref="KeyModifiers"/> that generated this event.
        /// </summary>
        public KeyModifiers Modifiers { get; }

        /// <summary>
        /// Gets the keyboard scan code of the key that generated this event.
        /// </summary>
        public int ScanCode { get; }

        /// <summary>
        /// Gets the <see cref="KeyEvent"/> that generated this event.
        /// </summary>
        public KeyEvent Event { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="KeyModifiers.Control" /> is pressed.
        /// </summary>
        public bool Control => Modifiers.HasFlag(KeyModifiers.Control);

        /// <summary>
        /// Gets a value indicating whether <see cref="KeyModifiers.Shift" /> is pressed.
        /// </summary>
        public bool Shift => Modifiers.HasFlag(KeyModifiers.Shift);

        /// <summary>
        /// Gets a value indicating whether <see cref="KeyModifiers.Super" /> is pressed.
        /// </summary>
        public bool Super => Modifiers.HasFlag(KeyModifiers.Super);
    }
}
