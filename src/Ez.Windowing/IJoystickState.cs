using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing
{
    /// <summary>
    /// Describes the state of a joystick.
    /// </summary>
    public interface IJoystickState
    {
        /// <summary>
        /// Gets the identity of the joystick this state describes.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Gets the name of the joystick this state describes.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the number of buttons on the joystick this state describes.
        /// </summary>
        int ButtonCount { get; }

        /// <summary>
        /// Gets the number of axes on the joystick this state describes.
        /// </summary>
        int AxisCount { get; }

        /// <summary>
        /// Gets the number of hats on the joystick this state describes.
        /// </summary>
        int HatCount { get; }

        /// <summary>
        /// Gets a <see cref="Hat"/> that describing the state of a hat.
        /// </summary>
        /// <param name="index">The index of the hat to check.</param>
        /// <returns>A <see cref="Hat"/> describing the hat state.</returns>
        Hat GetHat(int index);

        /// <summary>
        /// Gets a <see cref="Hat"/> that describing the previous state of a hat.
        /// </summary>
        /// <param name="index">The index of the hat to check.</param>
        /// <returns>A <see cref="Hat"/> describing the hat state.</returns>
        Hat GetHatPrevious(int index);

        /// <summary>
        /// Gets a <see cref="bool"/> that describing the state of a button in the current frame.
        /// </summary>
        /// <param name="index">The index of the button to check.</param>
        /// <returns><c>true</c> if the button is down; <c>false</c> otherwise.</returns>
        bool IsButtonDown(int index);

        /// <summary>
        /// Gets a <see cref="bool"/> that describing whether the button was down in the previous frame.
        /// </summary>
        /// <param name="index">The index of the button.</param>
        /// <returns>Returns true if the button was down, or false if the button was not down.</returns>
        bool IsButtonDownPrevious(int index);

        /// <summary>
        /// Gets a <see cref="bool"/> that describing whether the button is up after being down.
        /// </summary>
        /// <param name="index">The index of the button.</param>
        /// <returns>Returns true if the button is Up and in the previous frame it was down; <c>false</c> otherwise.</returns>
        bool IsButtonClick(int index);

        /// <summary>
        /// Gets a <see cref="bool"/> that describing whether the button is up after being down, but only return true in one code per frame.
        /// </summary>
        /// <param name="index">The index of the button.</param>
        /// <param name="code">The code of this call.</param>
        /// <returns>Returns true if the button is Up and in the previous frame it was down and it is the first time in this frame that the code is used; <c>false</c> otherwise.</returns>
        bool IsSingleButtonClick(int index, int code);

        /// <summary>
        /// Gets a <see cref="float"/> between -1 and 1 describing the position of an axis.
        /// </summary>
        /// <param name="index">The index of the Axis to check.</param>
        /// <returns>A <see cref="float"/> between -1 and 1 describing the position of the axis.</returns>
        float GetAxis(int index);

        /// <summary>
        /// Gets a <see cref="float"/> between -1 and 1 describing the previous position of an axis.
        /// </summary>
        /// <param name="index">The index of the Axis to check.</param>
        /// <returns>A <see cref="float"/> between -1 and 1 describing the position of the axis.</returns>
        float GetAxisPrevious(int index);

        /// <summary>
        /// Gets an immutable snapshot of this JoystickState.
        /// This can be used to save the current joystick state for comparison later on.
        /// </summary>
        /// <returns>Returns an immutable snapshot of this JoystickState.</returns>
        IJoystickState GetSnapshot();

        /// <summary>
        /// Hats for debugin .
        /// </summary>
        IReadOnlyList<Hat> Hats { get; }

        /// <summary>
        /// Previous Hats for debugin .
        /// </summary>
        IReadOnlyList<Hat> PreviousHats { get; }

        /// <summary>
        /// Buttons for debugin.
        /// </summary>
        IReadOnlyList<bool> Buttons { get; }

        /// <summary>
        /// Previous buttons for degugin.
        /// </summary>
        IReadOnlyList<bool> PreviousButtons { get; }

        /// <summary>
        /// Axis for debugin.
        /// </summary>
        IReadOnlyList<float> Axis { get; }

        /// <summary>
        /// Previous Axis for debugin.
        /// </summary>
        IReadOnlyList<float> PreviousAxis { get; }
    }
}
