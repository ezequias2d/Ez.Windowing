using System;
using System.Drawing;

namespace Ez.Windowing
{
    /// <summary>
    /// Describes properties to use for window creation.
    /// </summary>
    public struct WindowCreateInfo
    {
        /// <summary>
        /// Creates a new instance of <see cref="WindowCreateInfo"/> struct.
        /// </summary>
        /// <param name="position">The window position.</param>
        /// <param name="size">The window size.</param>
        /// <param name="initialState">The window state.</param>
        /// <param name="title">The window title.</param>
        /// <param name="api">The graphics API.</param>
        /// <param name="cursorMode">The cursor mode.</param>
        /// <param name="windowBorder">The window border.</param>
        /// <param name="opacity">The window opacity.</param>
        /// <param name="multisamples">The number of samples in window.</param>
        /// <param name="apiVersion">The version of the graphics API.</param>
        /// <param name="flags">The context flags.</param>
        /// <param name="profile">The context profile.</param>
        /// <param name="defaultCursorShape">The default cursor shape.</param>
        public WindowCreateInfo(
            Point position,
            Size size,
            WindowState initialState,
            string title,
            ContextAPI api,
            CursorMode cursorMode = CursorMode.Visible,
            WindowBorder windowBorder = WindowBorder.Resizable,
            float opacity = 1f,
            byte multisamples = 16,
            Version apiVersion = default,
            ContextFlags flags = ContextFlags.Default,
            ContextProfile profile = ContextProfile.Core,
            CursorShape defaultCursorShape = CursorShape.Arrow)
        {
            (Position, Size, WindowState, Title, WindowBorder, Multisamples, API, APIVersion, Flags, Profile, CursorMode, Opacity, DefaultCursorShape) =
                (position, size, initialState, title, windowBorder, multisamples, api, apiVersion, flags, profile, cursorMode, opacity, defaultCursorShape);

            APIVersion ??= new Version(3, 3);
        }

        /// <summary>
        /// Gets or sets a value representing the window position.
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Gets or sets a value representing the window size.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Gets or sets a value representing the window title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value representing the window state.
        /// </summary>
        public WindowState WindowState { get; set; }

        /// <summary>
        /// Gets or sets a value representing the window border.
        /// </summary>
        public WindowBorder WindowBorder { get; set; }

        /// <summary>
        /// Gets or sets a value representing the current graphics API.
        /// </summary>
        public ContextAPI API { get; set; }

        /// <summary>
        /// Gets or sets a value representing the current version of the graphics API.
        /// <para>
        ///     If null, OpenGL 3.3 is selected by default.
        /// </para>
        /// </summary>
        public Version APIVersion { get; set; }

        /// <summary>
        /// Gets or sets a value representing the current graphics profile flags.
        /// </summary>
        public ContextFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets a value representing the current graphics API profile.
        /// </summary>
        public ContextProfile Profile { get; set; }

        /// <summary>
        /// Gets or sets a value representing the cursor mode.
        /// </summary>
        public CursorMode CursorMode { get; set; }

        /// <summary>
        /// Gets or sets the default cursor shape in <see cref="IWindow.DefaultCursor"/>.
        /// </summary>
        public CursorShape DefaultCursorShape { get; set; }

        /// <summary>
        /// Gets or sets a value representing the window opacity.
        /// </summary>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets or sets a value representing the number of samples in window.
        /// </summary>
        public byte Multisamples { get; set; }
    }
}
