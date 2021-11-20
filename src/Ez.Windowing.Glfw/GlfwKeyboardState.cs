using System;

using Ez.Memory;

namespace Ez.Windowing.GLFW
{
    internal class GlfwKeyboardState : IKeyboardState, IDisposable
    {
        private readonly MemoryBlock _mb;
        private PinnedMemory<KeyEvent> _keyEvents;
        private PinnedMemory<KeyEvent> _previousKeyEvents;

        internal GlfwKeyboardState()
        {
            var keyEventsLength = ((int)Key.LastKey + 1);
            var size = sizeof(KeyEvent) * keyEventsLength * 2;
            _mb = MemoryBlockPool.Get(size);

            _keyEvents = _mb.AllocPinnedMemory<KeyEvent>(keyEventsLength);
            _previousKeyEvents = _mb.AllocPinnedMemory<KeyEvent>(keyEventsLength);
        }

        internal GlfwKeyboardState(GlfwKeyboardState keyboardState) : this()
        {
            MemUtil.Copy(_mb.Ptr, keyboardState._mb.Ptr, _mb.TotalSize);
        }

        ~GlfwKeyboardState() => Dispose(false);

        public KeyEvent this[Key key]
        {
            get => _keyEvents[(int)key];
            internal set 
            {
                _keyEvents[(int)key] = value;

                switch (value)
                {
                    case KeyEvent.Down:
                        IsAnyKeyDown = true;
                        break;
                    case KeyEvent.Up:
                        IsAnyKeyUp = true;
                        break;
                    case KeyEvent.Repeat:
                        IsAnyKeyRepeat = true;
                        break;
                }
            }
        }

        public bool IsAnyKeyDown { get; private set; }

        public bool IsAnyKeyUp { get; private set; }

        public bool IsAnyKeyRepeat { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                _mb.Dispose();
        }

        internal void Reset()
        {
            MemUtil.Set(_keyEvents.Ptr, KeyEvent.None, _keyEvents.Count);
            MemUtil.Set(_previousKeyEvents.Ptr, KeyEvent.None, _previousKeyEvents.Count);

            IsAnyKeyDown = false;
            IsAnyKeyRepeat = false;
            IsAnyKeyUp = false;
        }

        public KeyEvent GetKey(Key key) => _keyEvents[(int)key];

        public KeyEvent GetPreviousKey(Key key) => _previousKeyEvents[(int)key];

        public IKeyboardState GetSnapshot() => new GlfwKeyboardState(this);

        public bool IsKeyPressed(Key key) => _keyEvents[(int)key] == KeyEvent.Down;

        public bool IsKeyReleased(Key key) => _keyEvents[(int)key] == KeyEvent.Up;
    }
}
