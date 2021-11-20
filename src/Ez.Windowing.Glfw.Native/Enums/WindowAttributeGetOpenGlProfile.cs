using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native.Enums
{
    /// <summary>
    /// Used to get window related attributes.
    /// </summary>
    /// <seealso cref="Glfw.GetWindowAttrib(Window*, WindowAttributeGetOpenGlProfile)"/>
    public enum WindowAttributeGetOpenGlProfile
    {
        /// <summary>
        /// Indicates the OpenGL profile used by the context.
        /// This is <see cref="GraphicsLibraryFramework.OpenGlProfile.Core"/>
        /// or <see cref="GraphicsLibraryFramework.OpenGlProfile.Compat"/>
        /// if the context uses a known profile, or <see cref="GraphicsLibraryFramework.OpenGlProfile.Any"/>
        /// if the OpenGL profile is unknown or the context is an OpenGL ES context.
        /// Note that the returned profile may not match the profile bits of the context flags,
        /// as GLFW will try other means of detecting the profile when no bits are set.
        /// </summary>
        OpenGlProfile = WindowHintOpenGlProfile.OpenGlProfile
    }
}
