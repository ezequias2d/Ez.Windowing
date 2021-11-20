using System;
using System.Collections.Generic;
using System.Text;

using System.Numerics;
using System.Drawing;
namespace Ez.Windowing
{
    /// <summary>
    /// Describes the state of a mouse.
    /// </summary>
    public interface IMouseState
    {
        /// <summary>
        /// Gets a <see cref="Vector2"/> representing the absolute position of the pointer
        /// in the current frame, relative to the top-left corner of the contents of the window.
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Gets a <see cref="Vector2"/> representing the absolute position of the pointer
        /// in the previous frame, relative to the top-left corner of the contents of the window.
        /// </summary>
        Vector2 PreviousPosition { get; }

        /// <summary>
        /// Gets a <see cref="Vector2"/> representing the amount that the mouse moved since the last frame.
        /// This does not necessarily correspond to pixels, for example in the case of raw input.
        /// </summary>
        Vector2 Delta { get; }

        /// <summary>
        /// Get a Vector2 representing the position of the mouse wheel.
        /// </summary>
        Vector2 Scroll { get; }

        /// <summary>
        /// Get a Vector2 representing the position of the mouse wheel.
        /// </summary>
        Vector2 PreviousScroll { get; }

        /// <summary>
        /// Get a Vector2 representing the amount that the mouse wheel moved since the last frame.
        /// </summary>
        Vector2 ScrollDelta { get; }

        /// <summary>
        /// Gets a <see cref="KeyEvent" /> indicating whether the specified
        ///  <see cref="MouseButton" /> is up, down or pressed.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton" /> to check.</param>
        /// <returns> The <see cref="KeyEvent"/> of the <see cref="MouseButton"/>.</returns>
        KeyEvent this[MouseButton button] { get; }

        /// <summary>
        /// Gets a value indicating whether any key is in the <see cref="KeyEvent.Down"/> state.
        /// </summary>
        bool IsAnyButtonDown { get; }

        /// <summary>
        /// Gets a value indicating whether any key is in the <see cref="KeyEvent.Repeat"/> state.
        /// </summary>
        bool IsAnyButtonRepeat { get; }

        /// <summary>
        /// Gets a value indicating whether any key is in the <see cref="KeyEvent.Up"/> state.
        /// </summary>
        bool IsAnyButtonUp { get; }

        /// <summary>
        /// Gets the <see cref="KeyEvent"/> state of the <paramref name="button"/>.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> to get from.</param>
        /// <returns>A <see cref="KeyEvent"/> that represents the <see cref="MouseButton"/> state.</returns>
        KeyEvent GetButton(MouseButton button);

        /// <summary>
        /// Gets the previous <see cref="KeyEvent"/> state of the <paramref name="button"/>.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> to get from.</param>
        /// <returns>A <see cref="KeyEvent"/> that represents the previous <see cref="MouseButton"/> state.</returns>
        KeyEvent GetPreviousButton(MouseButton button);

        /// <summary>
        /// Gets a snapshot of this <see cref="IMouseState"/>.
        /// </summary>
        /// <returns>A snapshot of current <see cref="IMouseState"/>.</returns>
        IMouseState GetSnapshot();
    }
}
