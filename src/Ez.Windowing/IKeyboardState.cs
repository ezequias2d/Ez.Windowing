using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing
{
    /// <summary>
    /// Describes the state of a keyboard.
    /// </summary>
    public interface IKeyboardState
    {

        /// <summary>
        /// Gets a <see cref="KeyEvent" /> that indicates the current state of the specified <see cref="Key"/>.
        /// </summary>
        /// <param name="key">The <see cref="Key"/> to get <see cref="KeyEvent"/>.</param>
        /// <returns>The <see cref="KeyEvent"/> of <see cref="Key"/>.</returns>
        KeyEvent this[Key key] { get; }

        /// <summary>
        /// Gets a value indicating whether any <see cref="Key"/> is currently <see cref="KeyEvent.Down"/>.
        /// </summary>
        /// <value><c>true</c> if any key is down; otherwise, <c>false</c>.</value>
        bool IsAnyKeyDown { get; }

        /// <summary>
        /// Gets a value indicating whether any <see cref="Key"/> is currently <see cref="KeyEvent.Up"/>.
        /// </summary>
        /// <value><c>true</c> if any key is up; otherwise, <c>false</c>.</value>
        bool IsAnyKeyUp { get; }

        /// <summary>
        /// Gets a value indicating whether any <see cref="Key"/> is currently <see cref="KeyEvent.Repeat"/>.
        /// </summary>
        /// <value><c>true</c> if any key is up; otherwise, <c>false</c>.</value>
        bool IsAnyKeyRepeat { get; }

        /// <summary>
        /// Gets a <see cref="bool" /> indicating whether this key is currently down.
        /// </summary>
        /// <param name="key">The <see cref="Key"/> to check.</param>
        /// <returns><c>true</c> if <paramref name="key"/> is in the down state; otherwise, <c>false</c>.</returns>
        KeyEvent GetKey(Key key);

        /// <summary>
        /// Gets a <see cref="bool" /> indicating whether this key was down in the previous frame.
        /// </summary>
        /// <param name="key">The <see cref="Key"/> to check.</param>
        /// <returns><c>true</c> if <paramref name="key"/> was in the down state; otherwise, <c>false</c>.</returns>
        KeyEvent GetPreviousKey(Key key);

        /// <summary>
        ///     Gets whether the specified key is pressed in the current frame but released in the previous frame.
        /// </summary>
        /// <remarks>
        ///     "Frame" refers to invocations of <see cref="IWindow.ProcessEvents()"/> here.
        /// </remarks>
        /// <param name="key">The <see cref="Key"/> to check.</param>
        /// <returns>True if the key is pressed in this frame, but not the last frame.</returns>
        bool IsKeyPressed(Key key);

        /// <summary>
        ///     Gets whether the specified key is released in the current frame but pressed in the previous frame.
        /// </summary>
        /// <remarks>
        ///     "Frame" refers to invocations of <see cref="IWindow.ProcessEvents()"/> here.
        /// </remarks>
        /// <param name="key">The <see cref="Key">key</see> to check.</param>
        /// <returns>True if the key is released in this frame, but pressed the last frame.</returns>
        bool IsKeyReleased(Key key);

        /// <summary>
        /// Gets an immutable snapshot of this KeyboardState.
        /// This can be used to save the current keyboard state for comparison later on.
        /// </summary>
        /// <returns>Returns an immutable snapshot of this KeyboardState.</returns>
        IKeyboardState GetSnapshot();
    }
}
