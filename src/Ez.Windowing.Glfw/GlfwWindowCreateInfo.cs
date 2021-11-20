using System;
namespace Ez.Windowing.GLFW
{
    /// <summary>
    /// Describes properties to use for glfw window creation.
    /// </summary>
    public struct GlfwWindowCreateInfo
    {
        /// <summary>
        /// The monitor the window is on.
        /// </summary>
        public IntPtr MonitorHandle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this window is event-driven.
        /// An event-driven window will wait for events before updating/rendering.
        /// It is useful for non-game applications, where the program only needs to do any 
        /// processing after the user inputs something.
        /// </summary>
        public bool IsEventDriven { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="GlfwWindowCreateInfo"/> struct.
        /// </summary>
        /// <param name="monitorHandle"></param>
        /// <param name="isEventDriven"></param>
        public GlfwWindowCreateInfo(IntPtr monitorHandle, bool isEventDriven)
        {
            MonitorHandle = monitorHandle;
            IsEventDriven = isEventDriven;
        }

        /// <summary>
        /// A default value for this struct.
        /// </summary>
        public static readonly GlfwWindowCreateInfo Default =
            new(IntPtr.Zero, false);
    }
}
