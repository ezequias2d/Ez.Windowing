using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ez.Windowing
{
    /// <summary>
    /// Extensions for <see cref="IWindow"/> instances.
    /// </summary>
    public static class WindowExtensions
    {
        /// <summary>
        /// Computes the location of the specified
        /// client point into screen coordinates.
        /// </summary>
        /// <param name="window">The window to compute from.</param>
        /// <param name="position">The client coordinate 
        /// <see cref="Point"/> to convert.</param>
        /// <returns>A <see cref="Point"/> that represents the converted 
        /// <see cref="Point"/>, <paramref name="position"/>, in screen
        /// coordinates.</returns>
        public static Point PointToScreen(this IWindow window, in Point position)
        {
            var wp = window.Position;
            return new Point(wp.X + position.X, wp.Y + position.Y);
        }

        /// <summary>
        /// Computes the location of the specified
        /// client point into screen coordinates.
        /// </summary>
        /// <param name="window">The window to compute from.</param>
        /// <param name="position">The client coordinate 
        /// <see cref="Vector2"/> to convert.</param>
        /// <returns>A <see cref="Vector2"/> that represents the converted 
        /// <see cref="Vector2"/>, <paramref name="position"/>, in screen
        /// coordinates.</returns>
        public static Vector2 PointToScreen(this IWindow window, in Vector2 position)
        {
            var wp = window.Position;
            return position + new Vector2(wp.X, wp.Y);
        }

        /// <summary>
        /// Computes the location of the specified
        /// screen point into client coordinates.
        /// </summary>
        /// <param name="window">The window to compute for.</param>
        /// <param name="position">The screen coordinate 
        /// <see cref="Point"/> to convert.</param>
        /// <returns>A <see cref="Point"/> that represents the converted 
        /// <see cref="Point"/>, <paramref name="position"/>, in client
        /// coordinates.</returns>
        public static Point PointToClient(this IWindow window, in Point position)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
