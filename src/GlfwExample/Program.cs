using Ez.Windowing;
using Ez.Windowing.GLFW;
using System;
using System.Drawing;
using System.Threading;

namespace GlfwExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var windowCreateInfo = new WindowCreateInfo(
                new Point(100, 100),
                new Size(640, 480), 
                WindowState.Normal, 
                "Glfw Example", 
                ContextAPI.OpenGL);

            var window = new GlfwWindow(null, windowCreateInfo, new GlfwWindowCreateInfo(IntPtr.Zero, false));
            window.TextInput += (w, args) =>
            {
                Console.Write($"{(char)args.Unicode}");
            };

            while (window.Exists)
            {
                using var result = window.BeginProcessEvents();
                Thread.Sleep(1);

                window.EndProcessEvents(result);
            }

            window.WaitClose();
        }
    }
}
