using Ez.Windowing.GLFW.Native;
using System;

using GCursorShape = Ez.Windowing.GLFW.Native.Enums.CursorShape;
namespace Ez.Windowing.GLFW
{
    internal class GlfwCursor : ICursor
    {
        private readonly Cursor _cursor;
        public GlfwCursor(CursorShape shape)
        {
            var s = shape switch
            {
                CursorShape.Arrow => GCursorShape.Arrow,
                CursorShape.IBeam => GCursorShape.IBeam,
                CursorShape.Crosshair => GCursorShape.Crosshair,
                CursorShape.Hand => GCursorShape.Hand,
                CursorShape.HResize => GCursorShape.HResize,
                CursorShape.VResize => GCursorShape.VResize,
                _ => throw new ArgumentOutOfRangeException(nameof(shape)),
            };
            _cursor = Glfw.CreateStandardCursor(s);
        }

        public unsafe GlfwCursor(int xhot, int yhot, int width, int height, void* imageData)
        {
            unsafe
            {
                var image = new Image(width, height, (IntPtr)imageData);
                _cursor = Glfw.CreateCursor(image, xhot, yhot);
            }
        }

        public void Dispose()
        {
            Glfw.DestroyCursor(_cursor);
        }

        public static implicit operator Cursor(GlfwCursor c) => c._cursor;
    }
}
