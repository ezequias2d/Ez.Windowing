using System;
using System.Collections.Generic;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

using Ez.Windowing.GLFW.Native.Enums;
using Ez.Memory;

namespace Ez.Windowing.GLFW.Native
{
    public static class Glfw
    {
        private const string LibraryName = "glfw3.dll";

        static Glfw()
        {
            NativeLibrary.SetDllImportResolver(typeof(Glfw).Assembly, (name, assembly, path) =>
            {
                if (name != LibraryName)
                    return IntPtr.Zero;

                return LoadLibrary("glfw", new Version(3, 3), assembly, path);
            });
        }

        private static IntPtr LoadLibrary(string libraryName, Version version, Assembly assembly, DllImportSearchPath? searchPath)
        {
            IEnumerable<string> GetNextVersion()
            {
                for (var i = 2; i >= 0; i--)
                    yield return version.ToString(i);
            }

            Func<string, string, string> libNameFormatter;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                libNameFormatter = (libName, ver) =>
                    libName + ".so" + (string.IsNullOrEmpty(ver) ? string.Empty : "." + ver);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                libNameFormatter = (libName, ver) =>
                    libName + (string.IsNullOrEmpty(ver) ? string.Empty : "." + ver) + ".dylib";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                libNameFormatter = (libName, ver) =>
                    libName + (string.IsNullOrEmpty(ver) ? string.Empty : ver) + ".dll";
            else
                return IntPtr.Zero;

            foreach (var ver in GetNextVersion())
                if (NativeLibrary.TryLoad(libNameFormatter(libraryName, ver), assembly, searchPath, out var handle))
                    return handle;

            return NativeLibrary.Load(libraryName, assembly, searchPath);
        }

        [DllImport(LibraryName, EntryPoint = "glfwInit")]
        public static extern bool Init();

        [DllImport(LibraryName, EntryPoint = "glfwTerminate")]
        public static extern void Terminate();

        [DllImport(LibraryName, EntryPoint = "glfwInitHint")]
        public static extern void InitHint(int hint, int value);

        [DllImport(LibraryName, EntryPoint = "glfwGetVersion")]
        public static extern void GetVersion(out int major, out int minor, out int revision);

        [DllImport(LibraryName, EntryPoint = "glfwGetVersionString", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetVersionString();

        public static string GetVersion() => MemUtil.GetUtf8String(GetVersionString());

        [DllImport(LibraryName, EntryPoint = "glfwGetError", CharSet = CharSet.Ansi)]
        private static extern ErrorCode GetError(out IntPtr description);

        public static (ErrorCode ErrorCode, string Description) GetError()
        {
            var code = GetError(out IntPtr data);
            return (code, MemUtil.GetUtf8String(data));
        }

        [DllImport(LibraryName, EntryPoint = "glfwGetMonitors")]
        private static extern IntPtr GetMonitors(out int count);

        public static ReadOnlyPinnedMemory<Monitor> GetMonitors()
        {
            var monitors = GetMonitors(out var count);
            return new ReadOnlyPinnedMemory<Monitor>(monitors, count);
        }

        [DllImport(LibraryName, EntryPoint = "glfwGetMonitorPos")]
        public static extern void GetMonitorPosition(Monitor monitor, out int x, out int y);

        [DllImport(LibraryName, EntryPoint = "glfwGetMonitorPhysicalSize")]
        public static extern void GetMonitorPhysicalSize(Monitor monitor, out int width, out int height);

        [DllImport(LibraryName, EntryPoint = "glfwGetMonitorContentScale")]
        public static extern void GetMonitorContentScale(Monitor monitor, out float xscale, out float yscale);

        [DllImport(LibraryName, EntryPoint = "glfwGetMonitorName")]
        private static extern IntPtr GetMonitorNamePtr(Monitor monitor);

        public static string GetMonitorName(Monitor monitor) =>
            MemUtil.GetUtf8String(GetMonitorNamePtr(monitor));

        [DllImport(LibraryName, EntryPoint = "glfwSetMonitorUserPointer")]
        public static extern void SetMonitorUserPointer(Monitor monitor, IntPtr pointer);

        [DllImport(LibraryName, EntryPoint = "glfwGetMonitorUserPointer")]
        public static extern IntPtr GetMonitorUserPointer(Monitor monitor);

        [DllImport(LibraryName, EntryPoint = "glfwGetVideoModes")]
        private static extern IntPtr GetVideoModesUnsafe(Monitor monitor, out int count);

        public static ReadOnlyPinnedMemory<VideoMode> GetVideoModes(Monitor monitor)
        {
            var ptr = GetVideoModesUnsafe(monitor, out int count);
            return new ReadOnlyPinnedMemory<VideoMode>(ptr, count);
        }

        [DllImport(LibraryName, EntryPoint = "glfwSetGamma")]
        public static extern void SetGamma(Monitor monitor, float gamma);

        [DllImport(LibraryName, EntryPoint = "glfwGetGammaRamp")]
        private static extern IntPtr GetGammaRampPtr(Monitor monitor);

        public static GammaRamp GetGammaRamp(Monitor monitor) =>
            MemUtil.GetRef<GammaRamp>(GetGammaRampPtr(monitor));

        [DllImport(LibraryName, EntryPoint = "glfwSetGammaRamp")]
        public static extern void SetGammaRamp(Monitor monitor, ref GammaRamp ramp);

        [DllImport(LibraryName, EntryPoint = "glfwDefaultWindowHints")]
        public static extern void DefaultWindowHints();

        [DllImport(LibraryName, EntryPoint = "glfwWindowHintString")]
        public static extern void WindowHintString(int hint, IntPtr value);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowSizeLimits")]
        public static extern void SetWindowSizeLimits(Window window, int minwidth, int minheight, int maxwidth, int maxheight);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowAspectRatio")]
        public static extern void SetWindowAspectRatio(Window window, int numer, int denom);

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowFrameSize")]
        public static extern void GetWindowFrameSize(Window window, out int left, out int top, out int right, out int bottom);

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowContentScale")]
        public static extern void GetWindowContentScale(Window window, out float xscale, out float yscale);

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowOpacity")]
        public static extern float GetWindowOpacity(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowOpacity")]
        public static extern void SetWindowOpacity(Window window, float opacity);

        [DllImport(LibraryName, EntryPoint = "glfwRequestWindowAttention")]
        public static extern void RequestWindowAttention(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowAttrib")]
        public static extern void SetWindowAttrib(Window window, WindowAttribute attrib, bool value);

        [DllImport(LibraryName, EntryPoint = "glfwRawMouseMotionSupported")]
        public static extern int RawMouseMotionSupported();

        [DllImport(LibraryName, EntryPoint = "glfwGetKeyName")]
        [return: MarshalAs(UnmanagedType.LPUTF8Str)]
        private static extern string GetKeyNameUnsafe(Keys key, int scancode);

        [DllImport(LibraryName, EntryPoint = "glfwGetKeyScancode")]
        public static extern int GetKeyScancode(Keys key);

        [DllImport(LibraryName, EntryPoint = "glfwGetKey")]
        public static extern InputAction GetKey(Window window, Keys key);

        [DllImport(LibraryName, EntryPoint = "glfwGetMouseButton")]
        public static extern InputAction GetMouseButton(Window window, MouseButton button);

        [DllImport(LibraryName, EntryPoint = "glfwGetCursorPos")]
        public static extern void GetCursorPosition(Window window, out double xpos, out double ypos);

        [DllImport(LibraryName, EntryPoint = "glfwSetCursorPos")]
        public static extern void SetCursorPos(Window window, double xpos, double ypos);

        [DllImport(LibraryName, EntryPoint = "glfwCreateCursor")]
        public static extern Cursor CreateCursor(Image image, int xhot, int yhot);

        [DllImport(LibraryName, EntryPoint = "glfwCreateStandardCursor")]
        public static extern Cursor CreateStandardCursor(CursorShape shape);

        [DllImport(LibraryName, EntryPoint = "glfwDestroyCursor")]
        public static extern void DestroyCursor(Cursor cursor);

        [DllImport(LibraryName, EntryPoint = "glfwSetCursor")]
        public static extern void SetCursor(Window window, Cursor cursor);

        [DllImport(LibraryName, EntryPoint = "glfwJoystickPresent")]
        public static extern bool JoystickPresent(int jid);

        [DllImport(LibraryName, EntryPoint = "glfwGetJoystickAxes")]
        private static extern IntPtr GetJoystickAxes(int jid, out int count);

        public static ReadOnlyPinnedMemory<float> GetJoystickAxes(int jid)
        {
            var ptr = GetJoystickAxes(jid, out var count);
            return new ReadOnlyPinnedMemory<float>(ptr, count);
        }

        [DllImport(LibraryName, EntryPoint = "glfwGetJoystickButtons")]
        public static extern IntPtr GetJoystickButtons(int jid, out int count);

        public static ReadOnlyPinnedMemory<JoystickInputAction> GetJoystickButtons(int jid)
        {
            var ptr = GetJoystickButtons(jid, out var count);
            return new ReadOnlyPinnedMemory<JoystickInputAction> (ptr, count);
        }

        [DllImport(LibraryName, EntryPoint = "glfwGetJoystickHats")]
        private static extern IntPtr GetJoystickHats(int jid, out int count);

        public static ReadOnlyPinnedMemory<JoystickHats> GetJoystickHats(int jid)
        {
            var ptr = GetJoystickHats(jid, out var count);
            return new ReadOnlyPinnedMemory<JoystickHats>(ptr, count);
        }

        [DllImport(LibraryName, EntryPoint = "glfwGetJoystickName")]
        [return: MarshalAs(UnmanagedType.LPUTF8Str)]
        public static extern string GetJoystickName(int jid);

        [DllImport(LibraryName, EntryPoint = "glfwGetJoystickGUID")]
        [return: MarshalAs(UnmanagedType.LPUTF8Str)]
        public static extern string GetJoystickGUID(int jid);

        [DllImport(LibraryName, EntryPoint = "glfwSetJoystickUserPointer")]
        public static extern void SetJoystickUserPointer(int jid, IntPtr ptr);

        [DllImport(LibraryName, EntryPoint = "glfwGetJoystickUserPointer")]
        public static extern IntPtr GetJoystickUserPointer(int jid);

        [DllImport(LibraryName, EntryPoint = "glfwJoystickIsGamepad")]
        public static extern bool JoystickIsGamepad(int jid);

        [DllImport(LibraryName, EntryPoint = "glfwUpdateGamepadMappings", CharSet = CharSet.Ansi)]
        public static extern bool UpdateGamepadMappings(string newMapping);

        [DllImport(LibraryName, EntryPoint = "glfwGetGamepadName")]
        [return: MarshalAs(UnmanagedType.LPUTF8Str)]
        public static extern string GetGamepadName(int jid);

        [DllImport(LibraryName, EntryPoint = "glfwGetGamepadState")]
        public static extern bool GetGamepadState(int jid, out GamepadState state);

        [DllImport(LibraryName, EntryPoint = "glfwGetTime")]
        public static extern double GetTime();

        [DllImport(LibraryName, EntryPoint = "glfwSetTime")]
        public static extern void SetTime(double time);

        [DllImport(LibraryName, EntryPoint = "glfwGetTimerValue")]
        public static extern long GetTimerValue();

        [DllImport(LibraryName, EntryPoint = "glfwGetTimerFrequency")]
        public static extern long GetTimerFrequency();

        [DllImport(LibraryName, EntryPoint = "glfwGetCurrentContext")]
        public static extern IntPtr GetCurrentContext();

        [DllImport(LibraryName, EntryPoint = "glfwSwapBuffers")]
        public static extern void SwapBuffers(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwSwapBuffers")]
        public static extern void SwapBuffers(IntPtr window);

        [DllImport(LibraryName, EntryPoint = "glfwExtensionSupported", CharSet = CharSet.Ansi)]
        public static extern bool ExtensionSupported(string extensionName);

        [DllImport(LibraryName, EntryPoint = "glfwGetProcAddress")]
        public static extern IntPtr GetProcAddress(IntPtr procName);

        [DllImport(LibraryName, EntryPoint = "glfwGetProcAddress", CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(string procName);

        [DllImport(LibraryName, EntryPoint = "glfwCreateWindow", CharSet = CharSet.Ansi)]
        public static extern Window CreateWindow(int width, int height, string title, Monitor monitor = default, Window share = default);

        [DllImport(LibraryName, EntryPoint = "glfwGetPrimaryMonitor")]
        public static extern Monitor GetPrimaryMonitor();

        [DllImport(LibraryName, EntryPoint = "glfwDestroyWindow")]
        public static extern void DestroyWindow(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwFocusWindow")]
        public static extern void FocusWindow(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwGetFramebufferSize")]
        public static extern void GetFramebufferSize(Window window, out int width, out int height);

        [DllImport(LibraryName, EntryPoint = "glfwGetInputMode")]
        public static extern CursorModeValue GetInputMode(Window window, CursorStateAttribute mode);

        [DllImport(LibraryName, EntryPoint = "glfwGetInputMode")]
        public static extern int GetInputMode(Window window, StickyAttributes mode);

        [DllImport(LibraryName, EntryPoint = "glfwGetInputMode")]
        public static extern int GetInputMode(Window window, LockKeyModAttribute mode);

        [DllImport(LibraryName, EntryPoint = "glfwRestoreWindow")]
        public static extern void RestoreWindow(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwGetVideoMode")]
        private static extern IntPtr GetVideoModeUnsafe(Monitor monitor);

        public static VideoMode GetVideoMode(Monitor monitor) =>
            MemUtil.GetRef<VideoMode>(GetVideoModeUnsafe(monitor));

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowAttrib")]
        public static extern bool GetWindowAttrib(Window window, WindowAttributeGetBool attribute);

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowAttrib")]
        public static extern int GetWindowAttrib(Window window, WindowAttributeGetInt attribute);

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowAttrib")]
        public static extern ClientAPI GetWindowAttrib(Window window, WindowAttributeGetClientApi attribute);

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowAttrib")]
        public static extern ContextAPI GetWindowAttrib(Window window, WindowAttributeGetContextApi attribute);

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowAttrib")]
        public static extern OpenGLProfile GetWindowAttrib(Window window, WindowAttributeGetOpenGlProfile attribute);

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowAttrib")]
        public static extern ReleaseBehavior GetWindowAttrib(Window window, WindowAttributeGetReleaseBehavior attribute);

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowAttrib")]
        public static extern Robustness GetWindowAttrib(Window window, WindowAttributeGetRobustness attribute);

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowSize")]
        public static extern void GetWindowSize(Window window, out int width, out int height);

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowPos")]
        public static extern void GetWindowPosition(Window window, out int x, out int y);

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowMonitor")]
        public static extern Monitor GetWindowMonitor(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwHideWindow")]
        public static extern void HideWindow(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwIconifyWindow")]
        public static extern void IconifyWindow(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwMakeContextCurrent")]
        public static extern void MakeContextCurrent(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwMakeContextCurrent")]
        public static extern void MakeContextCurrent(IntPtr window);

        [DllImport(LibraryName, EntryPoint = "glfwMaximizeWindow")]
        public static extern void MaximizeWindow(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwGetWin32Window")]
        public static extern IntPtr GetWin32Window(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwGetCocoaWindow")]
        public static extern IntPtr GetCocoaWindow(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwGetX11Window")]
        public static extern uint GetX11Window(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwGetX11Display")]
        public static extern IntPtr GetX11Display();

        [DllImport(LibraryName, EntryPoint = "glfwGetGLXWindow")]
        public static extern uint GetGLXWindow(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwGetWaylandWindow")]
        public static extern IntPtr GetWaylandWindow(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwGetWaylandDisplay")]
        public static extern IntPtr GetWaylandDisplay();

        [DllImport(LibraryName, EntryPoint = "glfwPollEvents")]
        public static extern void PollEvents();

        [DllImport(LibraryName, EntryPoint = "glfwPostEmptyEvent")]
        public static extern void PostEmptyEvent();

        [DllImport(LibraryName, EntryPoint = "glfwWindowHint")]
        public static extern void WindowHint(WindowHintInt hint, int value);

        [DllImport(LibraryName, EntryPoint = "glfwWindowHint")]
        public static extern void WindowHint(WindowHintBool hint, bool value);

        [DllImport(LibraryName, EntryPoint = "glfwWindowHint")]
        public static extern void WindowHint(WindowHintClientApi hint, ClientAPI value);

        [DllImport(LibraryName, EntryPoint = "glfwWindowHint")]
        public static extern void WindowHint(WindowHintReleaseBehavior hint, ReleaseBehavior value);

        [DllImport(LibraryName, EntryPoint = "glfwWindowHint")]
        public static extern void WindowHint(WindowHintContextApi hint, ContextAPI value);

        [DllImport(LibraryName, EntryPoint = "glfwWindowHint")]
        public static extern void WindowHint(WindowHintRobustness hint, Robustness value);

        [DllImport(LibraryName, EntryPoint = "glfwWindowHint")]
        public static extern void WindowHint(WindowHintOpenGlProfile hint, OpenGLProfile value);

        [DllImport(LibraryName, EntryPoint = "glfwWindowShouldClose")]
        public static extern bool WindowShouldClose(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowUserPointer")]
        public static extern void SetWindowUserPointer(Window window, IntPtr pointer);

        [DllImport(LibraryName, EntryPoint = "glfwGetWindowUserPointer")]
        public static extern IntPtr GetWindowUserPointer(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwSetCharCallback")]
        public static extern IntPtr SetCharCallback(Window window, GlfwCallbacks.CharCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetCharModsCallback")]
        public static extern IntPtr SetCharModsCallback(Window window, GlfwCallbacks.CharModsCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetCursorEnterCallback")]
        public static extern IntPtr SetCursorEnterCallback(Window window, GlfwCallbacks.CursorEnterCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetCursorPosCallback")]
        public static extern IntPtr SetCursorPosCallback(Window window, GlfwCallbacks.CursorPosCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetDropCallback")]
        public static extern IntPtr SetDropCallback(Window window, GlfwCallbacks.DropCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetErrorCallback")]
        public static extern IntPtr SetErrorCallback(GlfwCallbacks.ErrorCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetInputMode")]
        public static extern void SetInputMode(Window window, CursorStateAttribute mode, CursorModeValue value);

        [DllImport(LibraryName, EntryPoint = "glfwSetInputMode")]
        public static extern void SetInputMode(Window window, StickyAttributes mode, int value);

        [DllImport(LibraryName, EntryPoint = "glfwSetInputMode")]
        public static extern void SetInputMode(Window window, LockKeyModAttribute mode, bool value);

        [DllImport(LibraryName, EntryPoint = "glfwSetJoystickCallback")]
        public static extern IntPtr SetJoystickCallback(GlfwCallbacks.JoystickCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetKeyCallback")]
        public static extern IntPtr SetKeyCallback(Window window, GlfwCallbacks.KeyCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetScrollCallback")]
        public static extern IntPtr SetScrollCallback(Window window, GlfwCallbacks.ScrollCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetMonitorCallback")]
        public static extern IntPtr SetMonitorCallback(GlfwCallbacks.MonitorCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetMouseButtonCallback")]
        public static extern IntPtr SetMouseButtonCallback(Window window, GlfwCallbacks.MouseButtonCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowCloseCallback")]
        public static extern IntPtr SetWindowCloseCallback(Window window, GlfwCallbacks.WindowCloseCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowFocusCallback")]
        public static extern IntPtr SetWindowFocusCallback(Window window, GlfwCallbacks.WindowFocusCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowIcon")]
        public static extern void SetWindowIcon(Window window, int count, in Image images);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowIconifyCallback")]
        public static extern IntPtr SetWindowIconifyCallback(Window window, GlfwCallbacks.WindowIconifyCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowMaximizeCallback")]
        public static extern IntPtr SetWindowMaximizeCallback(Window window, GlfwCallbacks.WindowMaximizeCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetFramebufferSizeCallback")]
        public static extern IntPtr SetFramebufferSizeCallback(Window window, GlfwCallbacks.FramebufferSizeCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowContentScaleCallback")]
        public static extern IntPtr SetWindowContentScaleCallback(Window window, GlfwCallbacks.WindowContentScaleCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowTitle", CharSet = CharSet.Ansi)]
        public static extern void SetWindowTitle(Window window, string title);

        [DllImport(LibraryName, EntryPoint = "glfwShowWindow")]
        public static extern void ShowWindow(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowSize")]
        public static extern void SetWindowSize(Window window, int width, int height);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowSizeCallback")]
        public static extern IntPtr SetWindowSizeCallback(Window window, GlfwCallbacks.WindowSizeCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowShouldClose")]
        public static extern void SetWindowShouldClose(Window window, bool value);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowMonitor")]
        public static extern void SetWindowMonitor(Window window, Monitor monitor, int x, int y, int width, int height, int refreshRate);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowPos")]
        public static extern void SetWindowPosition(Window window, int x, int y);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowPosCallback")]
        public static extern IntPtr SetWindowPosCallback(Window window, GlfwCallbacks.WindowPosCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSetWindowRefreshCallback")]
        public static extern IntPtr SetWindowRefreshCallback(Window window, GlfwCallbacks.WindowRefreshCallback callback);

        [DllImport(LibraryName, EntryPoint = "glfwSwapInterval")]
        public static extern void SwapInterval(int interval);

        [DllImport(LibraryName, EntryPoint = "glfwWaitEvents")]
        public static extern void WaitEvents();

        [DllImport(LibraryName, EntryPoint = "glfwWaitEventsTimeout")]
        public static extern void WaitEventsTimeout(double timeout);

        [DllImport(LibraryName, EntryPoint = "glfwGetClipboardString")]
        [return: MarshalAs(UnmanagedType.LPUTF8Str)]
        public static extern string GetClipboardString(Window window);

        [DllImport(LibraryName, EntryPoint = "glfwSetClipboardString")]
        public static extern void SetClipboardString(Window window,
                                                            [MarshalAs(UnmanagedType.LPUTF8Str)] string data);

        [DllImport(LibraryName, EntryPoint = "glfwVulkanSupported")]
        public static extern bool VulkanSupported();

        [DllImport(LibraryName, EntryPoint = "glfwGetRequiredInstanceExtensions")]
        private static extern IntPtr GetRequiredInstanceExtensionsUnsafe(out uint count);

        public static string[] GetRequiredInstanceExtensions()
        {
            var ptr = GetRequiredInstanceExtensionsUnsafe(out var count);
            var aux = MemUtil.GetSpan<IntPtr>(ptr, (int)count);

            string[] extensions = new string[count];
            for (var i = 0; i < count; i++)
                extensions[i] = MemUtil.GetUtf8String(aux[i]);

            return extensions;
        }

        [DllImport(LibraryName, EntryPoint = "glfwGetInstanceProcAddress", CharSet = CharSet.Ansi)]
        public static extern IntPtr GetInstanceProcAddress(VkHandle instance, string procName);

        [DllImport(LibraryName, EntryPoint = "glfwGetPhysicalDevicePresentationSupport")]
        public static extern int GetPhysicalDevicePresentationSupport(VkHandle instance, VkHandle device, int queueFamily);

        [DllImport(LibraryName, EntryPoint = "glfwCreateWindowSurface")]
        public static extern int CreateWindowSurface(VkHandle instance, Window window, IntPtr allocator, out VkHandle surface);
    }
}
