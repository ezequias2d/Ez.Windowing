using System;
using System.Numerics;
using Ez.Memory;

namespace Ez.Windowing.GLFW
{
    internal class GlfwMouseState : IMouseState, IDisposable
    {
        private readonly MemoryBlock _mb;
        private Vector2 _scroll;
        private PinnedMemory<KeyEvent> _buttons;
        private PinnedMemory<KeyEvent> _previousButtons;
        private Vector2 _position;
        private bool _isDisposed;

        public GlfwMouseState()
        {
            var buttonsLength = (int)(MouseButton.Last + 1);
            _mb = MemoryBlockPool.Get(sizeof(KeyEvent) * buttonsLength * 2);

            _buttons = _mb.AllocPinnedMemory<KeyEvent>(buttonsLength);
            _previousButtons = _mb.AllocPinnedMemory<KeyEvent>(buttonsLength);
        }

        ~GlfwMouseState()
        {
            Dispose(false);
        }

        public GlfwMouseState(GlfwMouseState original) : this()
        {
            MemUtil.Copy(_mb.Ptr, original._mb.Ptr, _mb.TotalSize);

            PreviousPosition = original.PreviousPosition;
            Position = original.Position;
            
            PreviousScroll = original.PreviousScroll;
            Scroll = original.Scroll;

            IsAnyButtonDown = original.IsAnyButtonDown;
            IsAnyButtonRepeat = original.IsAnyButtonRepeat;
            IsAnyButtonUp = original.IsAnyButtonUp;
        }

        public KeyEvent this[MouseButton button]
        {
            get => _buttons[(int)button];
            internal set
            {
                _buttons[(int)button] = value;
                UpdateIsAny(value);
            }
        }

        public Vector2 Position
        {
            get => _position;
            internal set
            {
                _position = value;
                Delta = value - PreviousPosition;
            }
        }

        public Vector2 PreviousPosition { get; private set; }

        public Vector2 Delta { get; private set; }

        public Vector2 Scroll
        {
            get => _scroll;
            internal set
            {
                _scroll = value;
                ScrollDelta = value - PreviousScroll;
            }
        }

        public Vector2 PreviousScroll { get; internal set; }

        public Vector2 ScrollDelta { get; internal set; }

        public bool IsAnyButtonDown { get; private set; }

        public bool IsAnyButtonRepeat { get; private set; }

        public bool IsAnyButtonUp { get; private set; }

        public KeyEvent GetButton(MouseButton button) =>
            _buttons[(int)button];

        public KeyEvent GetPreviousButton(MouseButton button) =>
            _previousButtons[(int)button];

        public IMouseState GetSnapshot()
        {
            return new GlfwMouseState(this);
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;
            _isDisposed = true;

            if (disposing)
                MemoryBlockPool.Return(_mb);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void UpdateIsAny(KeyEvent keyEvent)
        {
            switch (keyEvent)
            {
                case KeyEvent.Down:
                    IsAnyButtonDown = true;
                    break;
                case KeyEvent.Repeat:
                    IsAnyButtonRepeat = true;
                    break;
                case KeyEvent.Up:
                    IsAnyButtonUp = true;
                    break;
            }
        }

        internal void Reset()
        {
            PreviousPosition = Position;
            PreviousScroll = Scroll;

            Position = Vector2.Zero;
            Scroll = Vector2.Zero;
            IsAnyButtonDown = IsAnyButtonRepeat = IsAnyButtonUp = false;

            _mb.Reset();
        }
    }
}
