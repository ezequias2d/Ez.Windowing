using Microsoft.Extensions.Logging;

using System;
using System.Drawing;
using System.Numerics;
using System.Threading;
using System.ComponentModel;

using Ez.Threading;
using Ez.Windowing.GLFW.Native;
using Ez.Windowing.GLFW.Native.Enums;
using Monitor = Ez.Windowing.GLFW.Native.Monitor;
using Ez.Graphics.Contexts;
using System.Linq;
using Ez.Graphics.Context;
using Ez.Graphics.Context.SwapchainSources;
using Ez.Memory;
using System.Threading.Tasks;

namespace Ez.Windowing.GLFW
{
    /// <inheritdoc/>
    public sealed class GlfwWindow : IWindow
    {
        private readonly ILogger _logger;
        private readonly GlfwThread _glfwThread;
        private readonly Window _handle;
        private readonly bool _isEventDriven;
        private readonly ManualResetEvent _closeEvent;
        private readonly GlfwMouseState _mouseState;
        private readonly GlfwKeyboardState _keyboardState;
        private readonly GlfwJoystickState[] _joystickStates;
        private readonly ReaderWriterLockSlim _joystickStatesSync;
        private readonly Monitor _monitor;

        #region callbacks
        private readonly GlfwCallbacks.WindowPosCallback _windowPosCallback;
        private readonly GlfwCallbacks.WindowSizeCallback _changeWindowSize;
        private readonly GlfwCallbacks.FramebufferSizeCallback _framebuffeerSizeCallback;
        private readonly GlfwCallbacks.WindowIconifyCallback _windowIconifyCallback;
        private readonly GlfwCallbacks.WindowMaximizeCallback _windowMaximizeCallback;
        private readonly GlfwCallbacks.WindowFocusCallback _windowFocusCallback;
        private readonly GlfwCallbacks.CharCallback _charCallback;
        private readonly GlfwCallbacks.ScrollCallback _scrollCallback;
        //private readonly GLFWCallbacks.MonitorCallback _monitorCallback;
        private readonly GlfwCallbacks.WindowRefreshCallback _windowRefreshCallback;
        private readonly GlfwCallbacks.WindowCloseCallback _windowCloseCallback;
        private readonly GlfwCallbacks.KeyCallback _keyCallback;
        private readonly GlfwCallbacks.CursorEnterCallback _cursorEnterCallback;
        private readonly GlfwCallbacks.MouseButtonCallback _mouseButtonCallback;
        private readonly GlfwCallbacks.CursorPosCallback _cursorPosCallback;
        private readonly GlfwCallbacks.DropCallback _dropCallback;
        private readonly GlfwCallbacks.JoystickCallback _joystickCallback;
        #endregion

        private readonly ReaderWriterPropertyWrapper<string> _title;
        private readonly ReaderWriterPropertyWrapper<WindowState> _windowState;
        private readonly ReaderWriterPropertyWrapper<WindowBorder> _windowBorder;
        private readonly ReaderWriterPropertyWrapper<int> _width;
        private readonly ReaderWriterPropertyWrapper<int> _height;
        private readonly ReaderWriterPropertyWrapper<int> _x;
        private readonly ReaderWriterPropertyWrapper<int> _y;
        private readonly ReaderWriterPropertyWrapper<CursorMode> _cursorMode;
        private readonly ReaderWriterPropertyWrapper<float> _opacity;
        private readonly ReaderWriterPropertyWrapper<bool> _isFocused;
        private readonly ReaderWriterPropertyWrapper<bool> _isExiting;
        private readonly ReaderWriterPropertyWrapper<bool> _exist;
        private readonly ReaderWriterPropertyWrapper<Size> _framebufferSize;
        private readonly ReaderWriterPropertyWrapper<GlfwCursor> _cursor;


        // resolution and position of window before fullscreen window mode.
        private int _widthBackup;
        private int _heightBackup;
        private int _xBackup;
        private int _yBackup;

        /// <inheritdoc/>
        public GlfwWindow(ILogger logger, in WindowCreateInfo createInfo, in GlfwWindowCreateInfo glfwCreateInfo)
        {
            _logger = logger;
            Glfw.SetErrorCallback((error, description) => 
            {
                logger.LogError($"ErrorCode: {error}\n{description}");
            });

            _glfwThread = GlfwThread.Instance;

            _mouseState = new GlfwMouseState();
            _keyboardState = new GlfwKeyboardState();
            _joystickStates = new GlfwJoystickState[16];
            _joystickStatesSync = new ReaderWriterLockSlim();

            if (glfwCreateInfo.MonitorHandle == IntPtr.Zero)
                _monitor = _glfwThread.Invoke(() => Glfw.GetPrimaryMonitor());
            else
                _monitor = glfwCreateInfo.MonitorHandle;

            _closeEvent = new ManualResetEvent(false);
            _isEventDriven = glfwCreateInfo.IsEventDriven;

            _title = new(createInfo.Title);
            _windowState = new(createInfo.WindowState);
            _windowBorder = new(createInfo.WindowBorder);
            _width = new(createInfo.Size.Width);
            _height = new(createInfo.Size.Height);
            _x = new(createInfo.Position.X);
            _y = new(createInfo.Position.Y);
            _cursorMode = new(createInfo.CursorMode);
            _opacity = new(createInfo.Opacity);
            _isFocused = new(false);
            _isExiting = new(false);
            _exist = new(true);
            _framebufferSize = new(createInfo.Size);
            _cursor = new ReaderWriterPropertyWrapper<GlfwCursor>(null);

            API = createInfo.API;
            APIVersion = createInfo.APIVersion;
            Flags = createInfo.Flags;
            Profile = createInfo.Profile;
            Multisamples = createInfo.Multisamples;


            #region callbacks

            // These lambdas must be assigned to fields to prevent them from being garbage collected
            _windowPosCallback = (wh, x, y) => Moved?.Invoke(this, new Point(x, y));
            _windowIconifyCallback = (wh, iconified) => WindowStadeChange?.Invoke(this, WindowState.Minimized);
            _windowMaximizeCallback = (wh, maximized) =>
            {
                WindowStadeChange?.Invoke(this, WindowState.Maximized);

            };
            _windowFocusCallback = (wh, focused) =>
            {
                IsFocused = focused;
                if (focused)
                    GotFocus?.Invoke(this);
                else
                    LostFocus?.Invoke(this);
            };
            _charCallback = (wh, codepoint) => TextInput?.Invoke(this, new TextInputEventArgs((int)codepoint));
            _scrollCallback = (wh, x, y) => MouseWheel?.Invoke(this, new MouseEventArgs(MouseButton.None, 0, new Vector2((float)x, (float)y), Vector2.Zero, KeyEvent.None));
            _windowRefreshCallback = wh => Refresh?.Invoke(this);
            // These must be assigned to fields even when they're methods
            _windowCloseCallback = wh =>
            {
                var c = new CancelEventArgs();
                Closing?.Invoke(this, c);
                if (c.Cancel)
                    Glfw.SetWindowShouldClose(_handle, false);
                else
                    Dispose(true);
            };
            _keyCallback = (wh, key, scancode, action, mods) =>
            {
                Key k = key.ToKey();
                KeyEvent keyEvent = action.ToKeyEvent();
                var args = new KeyEventArgs(k, mods.ToKeyModifiers(), scancode, keyEvent);

                _keyboardState[k] = keyEvent;
                switch (keyEvent)
                {
                    case KeyEvent.Down:
                        KeyDown?.Invoke(this, args);
                        break;
                    case KeyEvent.Repeat:
                        KeyRepeat?.Invoke(this, args);
                        break;
                    case KeyEvent.Up:
                        KeyUp?.Invoke(this, args);
                        break;
                }
            };
            _cursorEnterCallback = (wh, entered) =>
            {
                var state = MouseState;
                var args = new MouseEventArgs(MouseButton.None, 0, state.Delta, state.Position, KeyEvent.None);
                if (entered)
                    MouseEntered?.Invoke(this, args);
                else
                    MouseLeft?.Invoke(this, args);
            };
            _mouseButtonCallback = (wh, button, action, mods) =>
            {
                var b = button.ToMouseButton();
                var keyEvent = action.ToKeyEvent();
                var args = new MouseEventArgs(b, 0, Vector2.Zero, _mouseState.Position, keyEvent);

                _mouseState[b] = keyEvent;
                switch (keyEvent)
                {
                    case KeyEvent.Down:
                        MouseDown?.Invoke(this, args);
                        break;
                    case KeyEvent.Up:
                        MouseUp?.Invoke(this, args);
                        break;
                    case KeyEvent.Repeat:
                        MouseRepeat?.Invoke(this, args);
                        break;
                }
            };

            _cursorPosCallback = (wh, posX, posY) =>
            {
                _mouseState.Position = new Vector2((float)posX, (float)posY);
                MouseMove?.Invoke(this, new MouseEventArgs(MouseButton.None, 0, Vector2.Zero, _mouseState.Position, KeyEvent.None));
            };

            _dropCallback = (wh, count, paths) => Drop?.Invoke(this, new DropEventArgs((string[])paths.Clone()));

            _joystickCallback = (joy, eventCode) =>
            {
                switch (eventCode)
                {
                    case ConnectedState.Connected:
                        _joystickStatesSync.EnterWriteLock();
                        _joystickStates[joy] = new GlfwJoystickState(joy);
                        _joystickStatesSync.ExitWriteLock();
                        JoystickConnection?.Invoke(this, new JoystickEventArgs(joy, eventCode == ConnectedState.Connected));
                        break;
                    default:
                        JoystickConnection?.Invoke(this, new JoystickEventArgs(joy, eventCode == ConnectedState.Connected));
                        _joystickStatesSync.EnterWriteLock();
                        _joystickStates[joy] = null;
                        _joystickStatesSync.ExitWriteLock();
                        break;
                }
            };

            _changeWindowSize = (window, width, height) =>
            {
                _width.Value = width;
                _height.Value = height;
                SizeChanged?.Invoke(this, new Size(width, height));
            };

            _framebuffeerSizeCallback = (window, width, height) =>
            {
                _framebufferSize.Value = new Size(width, height);
                FramebufferResize?.Invoke(this, new Size(width, height));
            };

            #endregion


            _handle = _glfwThread.Invoke(() => {
                // Ensures that the specific thread for Glfw is the mainthread.
                if (!GlfwThread.IsMainThread)
                    throw new GlfwException("Can only create windows on the Glfw main thread. (Thread from which Glfw was first created).");

                // Set hints for building the window
                bool fullscreen = false;
                switch (WindowState)
                {
                    case WindowState.Hidden:
                        Glfw.WindowHint(WindowHintBool.Visible, false);
                        break;
                    case WindowState.Maximized:
                        Glfw.WindowHint(WindowHintBool.Maximized, true);
                        break;
                    case WindowState.Normal:
                        Glfw.WindowHint(WindowHintBool.Visible, true);
                        break;
                    case WindowState.FullScreen:
                        fullscreen = true;
                        break;
                }
                switch (API)
                {
                    case ContextAPI.NoAPI:
                        Glfw.WindowHint(WindowHintClientApi.ClientApi, ClientAPI.NoAPI);
                        break;
                    case ContextAPI.OpenGL:
                        Glfw.WindowHint(WindowHintClientApi.ClientApi, ClientAPI.OpenGL);
                        break;
                    case ContextAPI.OpenGLES:
                        Glfw.WindowHint(WindowHintClientApi.ClientApi, ClientAPI.OpenGLES);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(createInfo.API));
                }

                Glfw.WindowHint(WindowHintInt.ContextVersionMajor, APIVersion.Major);
                Glfw.WindowHint(WindowHintInt.ContextVersionMinor, APIVersion.Minor);

                switch (Profile)
                {
                    case ContextProfile.Any:
                        Glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGLProfile.Any);
                        break;
                    case ContextProfile.Compatability:
                        Glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGLProfile.Compat);
                        break;
                    case ContextProfile.Core:
                        Glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGLProfile.Core);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(createInfo.Profile));
                }

                Glfw.WindowHint(WindowHintBool.Focused, true);
                Glfw.WindowHint(WindowHintInt.Samples, Multisamples);

                switch (WindowBorder)
                {
                    case WindowBorder.Resizable:
                        Glfw.WindowHint(WindowHintBool.Resizable, true);
                        break;
                    case WindowBorder.Fixed:
                        Glfw.WindowHint(WindowHintBool.Resizable, false);
                        break;
                    case WindowBorder.Hidden:
                        Glfw.WindowHint(WindowHintBool.Decorated, false);
                        break;
                }

                // Create the window
                Native.Window handle = Glfw.CreateWindow(_width, _height, _title, fullscreen ? _monitor : Monitor.Empty);

                // Get the current window
                Glfw.GetWindowPosition(handle, out var x, out var y);
                _x.Value = x;
                _y.Value = y;

                Glfw.GetFramebufferSize(handle, out var width, out var height);
                _framebufferSize.Value = new Size(width, height);

                Glfw.SetInputMode(handle, LockKeyModAttribute.LockKeyMods, true);

                // Sets the callbacks.
                RegisterWindowCallbacks(this, handle);
                InitialiseJoystrickState(this);

                {
                    CursorModeValue mode = default;
                    switch (CursorMode)
                    {
                        case CursorMode.Visible:
                            mode = CursorModeValue.CursorNormal;
                            break;
                        case CursorMode.Hidden:
                            mode = CursorModeValue.CursorHidden;
                            break;
                        case CursorMode.Disabled:
                            mode = CursorModeValue.CursorDisabled;
                            break;
                    }
                    Glfw.SetInputMode(handle, CursorStateAttribute.Cursor, mode);
                }

                Glfw.SetWindowOpacity(handle, Opacity);
                return handle;
            });

            Position = createInfo.Position;
            if (_handle.IsEmpty)
            {
                Glfw.Terminate();
                throw new GlfwException("Error in initializing window.");
            }

            DefaultCursor = CreateCursor(createInfo.DefaultCursorShape);
            Cursor = DefaultCursor;
        }

        /// <inheritdoc/>
        ~GlfwWindow() => Dispose(false);

        #region Properties
        /// <inheritdoc/>
        public ContextAPI API { get; }

        /// <inheritdoc/>
        public Version APIVersion { get; }

        /// <inheritdoc/>
        public ContextFlags Flags { get; }

        /// <inheritdoc/>
        public ContextProfile Profile { get; }

        /// <inheritdoc/>
        public byte Multisamples { get; }

        /// <inheritdoc/>
        public int X
        {
            get => _x;
            set
            {
                if(X != value)
                {
                    _x.Value = value;
                    SetPosition(value, Y);
                }
            }
        }

        /// <inheritdoc/>
        public int Y
        {
            get => _y;
            set
            {
                if(Y != value)
                {
                    _y.Value = value;
                    SetPosition(X, value);
                }
            }
        }

        /// <inheritdoc/>
        public Point Position
        {
            get => new(X, Y);
            set
            {
                var xdiff = X != value.X;
                var ydiff = Y != value.X;

                if (xdiff)
                    _x.Value = value.X;

                if (ydiff)
                    _y.Value = value.Y;

                if(xdiff || ydiff)
                    SetPosition(value.X, value.Y);
            }
        }

        /// <inheritdoc/>
        public IMouseState MouseState => _mouseState;

        /// <inheritdoc/>
        public IJoystickState GetJoystickState(int index)
        {
            _joystickStatesSync.EnterReadLock();
            IJoystickState output = _joystickStates[index];
            _joystickStatesSync.ExitReadLock();
            return output;
        }

        /// <inheritdoc/>
        public int Width
        {
            get => _width;
            set
            {
                if(Width != value)
                {
                    _width.Value = value;
                    SetSize(value, _height);
                }
            }
        }

        /// <inheritdoc/>
        public int Height
        {
            get => _height;
            set
            {
                if(Height != value)
                {
                    _height.Value = value;
                    SetSize(_width, value);
                }
            }
        }

        /// <inheritdoc/>
        public Size Size
        {
            get => new(Width, Height);
            set
            {
                var wdiff = Width != value.Width;
                var hdiff = Height != value.Height;

                if (wdiff)
                    _width.Value = value.Width;

                if (hdiff)
                    _height.Value = value.Height;

                if (wdiff || hdiff)
                    SetSize(value.Width, value.Height);
            }
        }

        /// <inheritdoc/>
        public IntPtr Handle => _handle;

        /// <inheritdoc/>
        public string Title
        {
            get => _title;
            set
            {
                if(Title != value)
                {
                    _title.Value = value;
                    SetTitle(value);
                }
            }
        }

        /// <inheritdoc/>
        public WindowState WindowState
        {
            get => _windowState;
            set
            {
                if (WindowState != value)
                {
                    switch (WindowState)
                    {
                        case WindowState.FullScreen:
                            SetFullScreen(false);
                            break;
                        case WindowState.Maximized:
                        case WindowState.Minimized:
                            SetRestore();
                            break;
                        case WindowState.Hidden:
                            SetShow();
                            break;
                    }

                    switch (value)
                    {
                        case WindowState.FullScreen:
                            SetFullScreen(true);
                            break;
                        case WindowState.Minimized:
                            SetMinimized();
                            break;
                        case WindowState.Maximized:
                            SetMaximize();
                            break;
                        case WindowState.Hidden:
                            SetHidden();
                            break;
                    }

                    _windowState.Value = value;
                }
            }
        }

        /// <inheritdoc/>
        public WindowBorder WindowBorder
        {
            get => _windowBorder;
            set
            {
                Glfw.GetVersion(out var major, out var minor, out _);

                // It isn't possible to implement this in versions of GLFW older than 3.3,
                // as SetWindowAttrib didn't exist before then.
                if ((major == 3) && (minor < 3))
                    throw new GlfwException("Cannot be implemented in GLFW before version 3.3.");

                if (WindowBorder != value)
                {
                    switch (value)
                    {
                        case WindowBorder.Hidden:
                            SetBorderHidden();
                            break;
                        case WindowBorder.Resizable:
                            SetBorderResizable();
                            break;
                        case WindowBorder.Fixed:
                            SetBorderFixed();
                            break;
                    }

                    _windowBorder.Value = value;
                }

            }
        }

        /// <inheritdoc/>
        public bool IsDisposed { get; private set; }

        /// <inheritdoc/>
        public Rectangle Bounds => new(Position, Size);

        /// <inheritdoc/>
        public CursorMode CursorMode
        {
            get => _cursorMode;
            set
            {
                if (CursorMode != value)
                {
                    CursorModeValue mode = default;
                    switch (value)
                    {
                        case CursorMode.Visible:
                            mode = CursorModeValue.CursorNormal;
                            break;
                        case CursorMode.Hidden:
                            mode = CursorModeValue.CursorHidden;
                            break;
                        case CursorMode.Disabled:
                            mode = CursorModeValue.CursorDisabled;
                            break;
                    }
                    _glfwThread.Invoke(() => Glfw.SetInputMode(_handle, CursorStateAttribute.Cursor, mode));

                    _cursorMode.Value = value;
                }
            }
        }

        /// <inheritdoc/>
        public float Opacity
        {
            get => _opacity;
            set
            {
                if (Opacity != value)
                {
                    _glfwThread.Invoke(() => Glfw.SetWindowOpacity(_handle, value));
                    _opacity.Value = value;
                    
                }
            }
        }

        /// <inheritdoc/>
        public bool IsFocused { get => _isFocused; private set => _isFocused.Value = value; }

        /// <inheritdoc/>
        public bool IsExiting { get => _isExiting; set => _isExiting.Value = value; }

        /// <inheritdoc/>
        public bool Exists { get => _exist; private set => _exist.Value = value; }

        /// <inheritdoc/>
        public string ClipboardString 
        {
            get => _glfwThread.Invoke(() => Glfw.GetClipboardString(_handle));
            set => _glfwThread.Invoke(() => Glfw.SetClipboardString(_handle, value));
        }

        /// <inheritdoc/>
        public Size FramebufferSize => _framebufferSize;

        /// <inheritdoc/>
        public ICursor DefaultCursor { get; }

        /// <inheritdoc/>
        public ICursor Cursor 
        {
            get => _cursor.Value;
            set
            {
                if (value is not GlfwCursor cursor)
                    throw new ArgumentException("The cursor must be create with IWindow.CreateCursor");
                
                if(_cursor.Value != cursor)
                {
                    _cursor.Value = cursor;
                    SetShapeCursor(cursor);
                }
            }
        }

        #endregion

        #region events
        /// <inheritdoc/>
        public event Action<IWindow, Size> SizeChanged;

        /// <inheritdoc/>
        public event Action<IWindow, CancelEventArgs> Closing;

        /// <inheritdoc/>
        public event Action<IWindow> Closed;

        /// <inheritdoc/>
        public event Action<IWindow> GotFocus;

        /// <inheritdoc/>
        public event Action<IWindow> LostFocus;

        /// <inheritdoc/>
        public event Action<IWindow, WindowState> WindowStadeChange;

        /// <inheritdoc/>
        public event Action<IWindow, Point> Moved;

        /// <inheritdoc/>
        public event Action<IWindow, TextInputEventArgs> TextInput;

        /// <inheritdoc/>
        public event Action<IWindow, MouseEventArgs> MouseEntered;

        /// <inheritdoc/>
        public event Action<IWindow, MouseEventArgs> MouseLeft;

        /// <inheritdoc/>
        public event Action<IWindow, MouseEventArgs> MouseWheel;

        /// <inheritdoc/>
        public event Action<IWindow, MouseEventArgs> MouseMove;

        /// <inheritdoc/>
        public event Action<IWindow, MouseEventArgs> MouseDown;

        /// <inheritdoc/>
        public event Action<IWindow, MouseEventArgs> MouseUp;

        /// <inheritdoc/>
        public event Action<IWindow, MouseEventArgs> MouseRepeat;

        /// <inheritdoc/>
        public event Action<IWindow, KeyEventArgs> KeyDown;

        /// <inheritdoc/>
        public event Action<IWindow, KeyEventArgs> KeyUp;

        /// <inheritdoc/>
        public event Action<IWindow, KeyEventArgs> KeyRepeat;

        /// <inheritdoc/>
        public event Action<IWindow, DropEventArgs> Drop;

        /// <inheritdoc/>
        public event Action<IWindow, JoystickEventArgs> JoystickConnection;

        /// <inheritdoc/>
        public event Action<IWindow> Refresh;

        /// <inheritdoc/>
        public event Action<IWindow, Size> FramebufferResize;
        #endregion

        #region public methods

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public void WaitClose()
        {
            _closeEvent.WaitOne();
        }

        /// <inheritdoc/>
        public ISwapchainSource GetSwapchainSource()
        {
            if (OperatingSystem.IsWindows())
            {
                var hwnd = Glfw.GetWin32Window(Handle);
                return new Win32SwapchainSource(hwnd, Handle);
            }

            if (OperatingSystem.IsMacOS())
            {
                var nswindow = Glfw.GetCocoaWindow(Handle);
                return new NSWindowSwapchainSource(nswindow);
            }
            
            if(Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var window = (IntPtr)Glfw.GetX11Window(Handle);
                var display = Glfw.GetX11Display();
                if(Glfw.GetError().ErrorCode == ErrorCode.NoError)
                    return new XlibSwapchainSource(display, window);
                
                var surface = Glfw.GetWaylandWindow(Handle);
                display = Glfw.GetWaylandDisplay();

                if (Glfw.GetError().ErrorCode == ErrorCode.NoError)
                    return new WaylandSwapchainSource(display, surface);
            }

            throw new PlatformNotSupportedException();
        }

        /// <inheritdoc/>
        public void ProcessEvents()
        {
            if (!PreProcessEvents())
                return;

            _glfwThread.Invoke(() =>
            {
                ProcessInputEvents();

                if (_isEventDriven)
                    Glfw.WaitEventsTimeout(1);
                else
                    Glfw.PollEvents();
            });
        }

        /// <inheritdoc/>
        public Task BeginProcessEvents()
        {
            if (!PreProcessEvents())
                return Task.CompletedTask;

            return Task.Factory.FromAsync(_glfwThread.BeginInvoke(() =>
            {
                ProcessInputEvents();

                if (_isEventDriven)
                    Glfw.WaitEventsTimeout(1);
                else
                    Glfw.PollEvents();
            }), 
            (result) => 
            {
                var rd = (IAsyncResultDisposable)result;
                _glfwThread.EndInvoke(rd);
                rd.Dispose();
            });
        }

        /// <inheritdoc/>
        public OpenGLContext GetOpenGLContext() =>
            new(handle:Handle,
                getProcAddress: Glfw.GetProcAddress,
                makeCurrent: Glfw.MakeContextCurrent,
                getCurrentContext: Glfw.GetCurrentContext,
                clearCurrentContext: ()=> Glfw.MakeContextCurrent(IntPtr.Zero),
                swapBuffers: Glfw.SwapBuffers,
                setSyncToVerticalBlank: (b) => Glfw.SwapInterval(b ? 1 : 0),
                resizeSwapchain: (x, y) => Size = new Size((int)x, (int)y));

        /// <inheritdoc/>
        public VulkanRequiredExtensions GetVulkanRequiredExtensions() =>
            new(Glfw.GetRequiredInstanceExtensions(), Enumerable.Empty<string>());
        #endregion

        #region private methods
        private bool PreProcessEvents()
        {
            if (IsExiting)
            {
                //DestroyWindow();
                return false;
            }
            return true;
        }

        private void SetFullScreen(bool fullscreen)
        {
            if ((WindowState == WindowState.FullScreen) == fullscreen)
                return;

            if (fullscreen)
            {
                _glfwThread.Invoke(() =>
                {
                    Glfw.GetWindowPosition(_handle, out _xBackup, out _yBackup);
                    Glfw.GetWindowSize(_handle, out _widthBackup, out _heightBackup);

                    var videoMode = Glfw.GetVideoMode(_monitor);

                    Glfw.SetWindowMonitor(_handle, _monitor, 0, 0, videoMode.Width, videoMode.Height, videoMode.RefreshRate);
                });
            }
            else
                _glfwThread.Invoke(() =>
                    Glfw.SetWindowMonitor(_handle, Monitor.Empty, _xBackup, _yBackup, _widthBackup, _heightBackup, 0));
        }

        private void SetTitle(string value) =>
            _glfwThread.Invoke(() => Glfw.SetWindowTitle(_handle, value));

        private void SetMinimized() =>
            _glfwThread.Invoke(() => Glfw.IconifyWindow(_handle));

        private void SetMaximize() =>
            _glfwThread.Invoke(() => Glfw.MaximizeWindow(_handle));

        private void SetRestore() =>
            _glfwThread.Invoke(() => Glfw.RestoreWindow(_handle));

        private void SetPosition(int x, int y) =>
            _glfwThread.Invoke(() => Glfw.SetWindowPosition(_handle, x, y));

        private void SetSize(int width, int height) =>
            _glfwThread.Invoke(() => Glfw.SetWindowSize(_handle, width, height));

        private void SetShapeCursor(GlfwCursor cursor)
        {
            Glfw.SetCursor(_handle, cursor);
        }

        private void SetHidden() =>
            _glfwThread.Invoke(() => Glfw.HideWindow(_handle));

        private void SetShow() =>
            _glfwThread.Invoke(() => Glfw.ShowWindow(_handle));

        private void SetBorderHidden() =>
            _glfwThread.Invoke(() => Glfw.SetWindowAttrib(_handle, WindowAttribute.Decorated, false));

        private void SetBorderResizable() =>
            _glfwThread.Invoke(() =>
            {
                Glfw.SetWindowAttrib(_handle, WindowAttribute.Decorated, true);
                Glfw.SetWindowAttrib(_handle, WindowAttribute.Resizable, true);
            });

        private void SetBorderFixed() =>
            _glfwThread.Invoke(() =>
            {
                Glfw.SetWindowAttrib(_handle, WindowAttribute.Decorated, true);
                Glfw.SetWindowAttrib(_handle, WindowAttribute.Resizable, false);
            });

        private void DestroyWindow()
        {
            if (Exists)
            {
                Exists = false;
                _glfwThread.Invoke(() => Glfw.DestroyWindow(_handle));

                OnClosed();
            }
        }

        private void OnClosed()
        {
            Closed?.Invoke(this);
            IsExiting = true;
            _closeEvent.Set();
        }

        private void ProcessInputEvents()
        {
            _mouseState.Reset();
            _keyboardState.Reset();

            Glfw.GetCursorPosition(_handle, out double x, out double y);
            _mouseState.Position = new Vector2((float)x, (float)y);


            foreach (GlfwJoystickState joystickState in _joystickStates)
            {
                if (joystickState != null)
                    joystickState?.Update();
            }
        }

        private void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            if (disposing)
            {
                DestroyWindow();
                DefaultCursor.Dispose();

                foreach(var joystickState in _joystickStates)
                    joystickState?.Dispose();

                _keyboardState.Dispose();
                _mouseState.Dispose();
                _joystickStatesSync.Dispose();
                _title.Dispose();
                _windowState.Dispose();
                _windowBorder.Dispose();
                _width.Dispose();
                _height.Dispose();
                _x.Dispose();
                _y.Dispose();
                _cursorMode.Dispose();
                _opacity.Dispose();
                _isFocused.Dispose();
                _isExiting.Dispose();
                _framebufferSize.Dispose();
                _cursor.Dispose();
            }
        }

        private static void RegisterWindowCallbacks(GlfwWindow window, Native.Window handle)
        {
            Glfw.SetWindowPosCallback(handle, window._windowPosCallback);
            Glfw.SetWindowSizeCallback(handle, window._changeWindowSize);
            Glfw.SetWindowIconifyCallback(handle, window._windowIconifyCallback);
            Glfw.SetWindowMaximizeCallback(handle, window._windowMaximizeCallback);
            Glfw.SetWindowFocusCallback(handle, window._windowFocusCallback);
            Glfw.SetCharCallback(handle, window._charCallback);
            Glfw.SetScrollCallback(handle, window._scrollCallback);
            Glfw.SetWindowRefreshCallback(handle, window._windowRefreshCallback);
            Glfw.SetWindowCloseCallback(handle, window._windowCloseCallback);
            Glfw.SetKeyCallback(handle, window._keyCallback);

            Glfw.SetCursorEnterCallback(handle, window._cursorEnterCallback);
            Glfw.SetMouseButtonCallback(handle, window._mouseButtonCallback);
            Glfw.SetCursorPosCallback(handle, window._cursorPosCallback);
            Glfw.SetDropCallback(handle, window._dropCallback);
            Glfw.SetJoystickCallback(window._joystickCallback);
            
            Glfw.SetFramebufferSizeCallback(handle, window._framebuffeerSizeCallback);
        }

        private static void InitialiseJoystrickState(GlfwWindow window)
        {
            // Check for Joysticks that are connected at application launch
            for (int i = 0; i < window._joystickStates.Length; i++)
                if (Glfw.JoystickPresent(i))
                    window._joystickStates[i] = new GlfwJoystickState(i);
        }

        /// <inheritdoc/>
        public ICursor CreateCursor(CursorShape shape)
        {
            var cursor = _glfwThread.Invoke(() => new GlfwCursor(shape));
            return cursor;
        }

        /// <inheritdoc/>
        public ICursor CreateCursor(int xhot, int yhot, int width, int height, ReadOnlySpan<byte> imageData)
        {
            unsafe
            {
                byte* data = stackalloc byte[imageData.Length];
                MemUtil.Copy(data, imageData);
                var cursor = _glfwThread.Invoke(() => new GlfwCursor(xhot, yhot, width, height, data));
                return cursor;
            }
        }

        #endregion
    }
}
