namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// Defines event information for <see cref="Glfw.GetJoystickButtons(int, out int)"/> or <see cref="Glfw.GetJoystickHats(int)"/>.
    /// </summary>
    public enum JoystickInputAction : byte
    {
        /// <summary>
        /// The joystick button was released.
        /// </summary>
        Release = 0,

        /// <summary>
        /// The joystick button was pressed.
        /// </summary>
        Press = 1,
    }
}
