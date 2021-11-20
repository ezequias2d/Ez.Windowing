using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

using Ez.Windowing.GLFW.Native.Enums;
using Ez.Threading;
using Ez.Memory;

namespace Ez.Windowing.GLFW.Native
{
    internal class GlfwThread : ThreadMethodExecutor
    {
        public static GlfwThread Instance { get; }

        static GlfwThread()
        {
            Instance = new GlfwThread();
        }

        private static Thread _mainThread;

        private GlfwThread() : base(true)
        {
            Priority = ThreadPriority.Lowest;

            Invoke(Init);
        }

        private void Init()
        {
            _mainThread = Thread.CurrentThread;
            Glfw.Init();
            Glfw.SetErrorCallback((ErrorCode errorCode, string description) => throw new GlfwNativeException(description, errorCode));
        }

        public static bool IsMainThread => _mainThread == Thread.CurrentThread;
    }
}
