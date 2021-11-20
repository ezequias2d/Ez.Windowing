using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing
{
    /// <summary>
    /// Describes the graphics API you want the context to use.
    /// </summary>
    public enum ContextAPI
    {
        /// <summary>
        /// Indicates that an API has not been specifically requested for context creation.
        /// <para>
        ///     This is primarily for integrating an external API with this window, such as Vulkan.
        /// </para>
        /// </summary>
        NoAPI,
        /// <summary>
        /// Indicates that the context should be created for OpenGL ES.
        /// </summary>
        OpenGLES,
        /// <summary>
        /// Indicates that the context should be created for OpenGL.
        /// </summary>
        OpenGL
    }
}
