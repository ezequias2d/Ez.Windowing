using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing
{
    /// <summary>
    /// Defines the event data for the <see cref="IWindow.JoystickConnection"/> event.
    /// </summary>
    public readonly struct JoystickEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JoystickEventArgs"/> struct.
        /// </summary>
        /// <param name="joystickId">The ID of the joystick which triggered this event.</param>
        /// <param name="isConnected">
        /// A value indicating whether the joystick which triggered this event was connected.
        /// </param>
        public JoystickEventArgs(int joystickId, bool isConnected)
        {
            JoystickId = joystickId;
            IsConnected = isConnected;
        }

        /// <summary>
        /// Gets the Id of the joystick which triggered this event.
        /// </summary>
        public int JoystickId { get; }

        /// <summary>
        /// Gets a value indicating whether the joystick which triggered this event was connected.
        /// </summary>
        public bool IsConnected { get; }
    }
}
