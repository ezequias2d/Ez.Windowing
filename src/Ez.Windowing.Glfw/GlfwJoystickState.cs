using Ez.Memory;
using System;
using System.Collections.Generic;

using Ez.Windowing.GLFW.Native;
using Ez.Windowing.GLFW.Native.Enums;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Ez.Windowing.GLFW
{
    /// <inheritdoc/>
    internal class GlfwJoystickState : IJoystickState, IDisposable
    {
        private readonly MemoryBlock _mb;
        private readonly HashSet<(int code, int index)> _usedCodes;
        private readonly ReaderWriterLockSlim hashSetSync;

        internal GlfwJoystickState(int id)
        {
            ID = id;
            Name = Glfw.GetJoystickName(ID);

            HatCount = Glfw.GetJoystickHats(ID).Count;
            AxisCount = Glfw.GetJoystickAxes(ID).Count;
            ButtonCount = Glfw.GetJoystickButtons(ID).Count;

            // alloc memory block and earch respective array from them.
            var hatsSize = sizeof(Hat) * HatCount;
            var axisSize = sizeof(float) * AxisCount;
            var buttonSize = sizeof(bool) * ButtonCount;
            var totalSize = (hatsSize + axisSize + buttonSize) * 2;
            _mb = MemoryBlockPool.Get(totalSize);


            Hats = _mb.AllocPinnedMemory<Hat>(HatCount);
            PreviousHats = _mb.AllocPinnedMemory<Hat>(HatCount);

            Buttons = _mb.AllocPinnedMemory<bool>(ButtonCount);
            PreviousButtons = _mb.AllocPinnedMemory<bool>(ButtonCount);

            Axis = _mb.AllocPinnedMemory<float>(AxisCount);
            PreviousAxis = _mb.AllocPinnedMemory<float>(AxisCount);

            _usedCodes = new HashSet<(int, int)>();
            hashSetSync = new ReaderWriterLockSlim();
        }

        /// <inheritdoc/>
        ~GlfwJoystickState()
        {
            Dispose(false);
        }

        /// <inheritdoc/>
        public GlfwJoystickState(GlfwJoystickState original)
        {
            ID = original.ID;
            Name = original.Name;

            ButtonCount = original.ButtonCount;
            AxisCount = original.AxisCount;
            HatCount = original.HatCount;

            var size = ((sizeof(float) * AxisCount) + (sizeof(Hat) * HatCount) + (sizeof(bool) * ButtonCount)) * 2;
            _mb = MemoryBlockPool.Get(size);
            MemUtil.Copy(_mb.Ptr, original._mb.Ptr, size);

            Hats = _mb.AllocPinnedMemory<Hat>(HatCount);
            PreviousHats = _mb.AllocPinnedMemory<Hat>(HatCount);

            Buttons = _mb.AllocPinnedMemory<bool>(ButtonCount);
            PreviousButtons = _mb.AllocPinnedMemory<bool>(ButtonCount);

            Axis = _mb.AllocPinnedMemory<float>(AxisCount);
            PreviousAxis = _mb.AllocPinnedMemory<float>(AxisCount);

            _usedCodes = new HashSet<(int, int)>();
            hashSetSync = new ReaderWriterLockSlim();
        }

        public int ID { get; }

        public string Name { get; }

        public int ButtonCount { get; }

        public int AxisCount { get; }

        public int HatCount { get; }

        public PinnedMemory<Hat> Hats { get; }

        public PinnedMemory<Hat> PreviousHats { get; }

        public PinnedMemory<bool> Buttons { get; }

        public PinnedMemory<bool> PreviousButtons { get; }

        public PinnedMemory<float> Axis { get; }

        public PinnedMemory<float> PreviousAxis { get; }

        IReadOnlyList<Hat> IJoystickState.Hats => Hats;

        IReadOnlyList<Hat> IJoystickState.PreviousHats => PreviousHats;

        IReadOnlyList<bool> IJoystickState.Buttons => Buttons;

        IReadOnlyList<bool> IJoystickState.PreviousButtons => PreviousButtons;

        IReadOnlyList<float> IJoystickState.Axis => Axis;

        IReadOnlyList<float> IJoystickState.PreviousAxis => PreviousAxis;

        public float GetAxis(int index)
        {
            if (index < 0 || index >= AxisCount)
                throw new IndexOutOfRangeException($"The index is out of range.");
            return Axis[index];
        }

        public float GetAxisPrevious(int index)
        {
            if (index < 0 || index >= AxisCount)
                throw new IndexOutOfRangeException($"The index is out of range.");
            return PreviousAxis[index];
        }

        public Hat GetHat(int index)
        {
            if (index < 0 || index >= HatCount)
                throw new IndexOutOfRangeException($"The index is out of range.");
            return Hats[index];
        }

        public Hat GetHatPrevious(int index)
        {
            if (index < 0 || index >= AxisCount)
                throw new IndexOutOfRangeException($"The index is out of range.");
            return PreviousHats[index];
        }

        public IJoystickState GetSnapshot()
        {
            return new GlfwJoystickState(this);
        }

        public bool IsButtonDown(int index)
        {
            if (index < 0 || index >= ButtonCount)
                throw new IndexOutOfRangeException($"The index is out of range.");

            return Buttons[index];
        }

        public bool IsButtonDownPrevious(int index)
        {
            if (index < 0 || index >= ButtonCount)
                throw new IndexOutOfRangeException($"The index is out of range.");

            return PreviousButtons[index];
        }

        public bool IsButtonClick(int index)
        {
            if (index < 0 || index >= ButtonCount)
                throw new IndexOutOfRangeException($"The index is out of range.");

            return PreviousButtons[index] && !Buttons[index];
        }

        public bool IsSingleButtonClick(int index, int code)
        {
            if (index < 0 || index >= ButtonCount)
                throw new IndexOutOfRangeException($"The index is out of range.");

            try
            {
                hashSetSync.EnterUpgradeableReadLock();
                var pair = (code, index);
                if (!_usedCodes.Contains(pair) && PreviousButtons[index] && !Buttons[index])
                {
                    try
                    {
                        hashSetSync.EnterWriteLock();
                        _usedCodes.Add(pair);
                    }
                    finally
                    {
                        hashSetSync.ExitWriteLock();
                    }
                    return true;
                }
            }
            finally
            {
                hashSetSync.ExitUpgradeableReadLock();
            }

            return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                MemoryBlockPool.Return(_mb);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Update()
        {
            UpdateHats();

            UpdateAxes();

            UpdateButtons();

            try
            {
                hashSetSync.EnterWriteLock();
                _usedCodes.Clear();
            }
            finally
            {
                hashSetSync.ExitWriteLock();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateButtons()
        {
            var buttons = Glfw.GetJoystickButtons(ID);
            if (buttons.Count > 0)
            {
                try
                {
                    PreviousButtons.ReaderWriterLock.EnterWriteLock();
                    try
                    {
                        Buttons.ReaderWriterLock.EnterReadLock();
                        MemUtil.Copy<bool>(PreviousButtons, Buttons);
                    }
                    finally
                    {
                        Buttons.ReaderWriterLock.ExitReadLock();
                    }
                }
                finally
                {
                    PreviousButtons.ReaderWriterLock.ExitWriteLock();
                }

                try
                {
                    Buttons.ReaderWriterLock.EnterWriteLock();
                    MemUtil.Copy<bool, JoystickInputAction>(
                        Buttons, 
                        ((ReadOnlySpan<JoystickInputAction>)buttons)[..ButtonCount]);
                }
                finally
                {
                    Buttons.ReaderWriterLock.ExitWriteLock();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateAxes()
        {
            var axes = Glfw.GetJoystickAxes(ID);
            if (axes.Count > 0)
            {
                try
                {
                    PreviousAxis.ReaderWriterLock.EnterWriteLock();
                    try
                    {
                        Axis.ReaderWriterLock.EnterReadLock();
                        MemUtil.Copy<float>(PreviousAxis, Axis);
                    }
                    finally
                    {
                        Axis.ReaderWriterLock.ExitReadLock();
                    }
                }
                finally
                {
                    PreviousAxis.ReaderWriterLock.ExitWriteLock();
                }

                try
                {
                    Axis.ReaderWriterLock.EnterWriteLock();
                    MemUtil.Copy(Axis, ((ReadOnlySpan<float>)axes)[..AxisCount]);
                }
                finally
                {
                    Axis.ReaderWriterLock.ExitWriteLock();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateHats()
        {
            var hats = Glfw.GetJoystickHats(ID);
            if (hats.Count > 0)
            {
                try
                {
                    PreviousHats.ReaderWriterLock?.EnterWriteLock();
                    try
                    {
                        Hats.ReaderWriterLock?.EnterReadLock();
                        MemUtil.Copy<Hat>(PreviousHats, Hats);
                    }
                    finally
                    {
                        Hats.ReaderWriterLock?.ExitReadLock();
                    }
                }
                finally
                {
                    PreviousHats.ReaderWriterLock?.ExitWriteLock();
                }

                try
                {
                    Hats.ReaderWriterLock?.EnterWriteLock();
                    MemUtil.Copy<Hat, JoystickHats>(Hats, ((ReadOnlySpan<JoystickHats>)hats)[..HatCount]);
                }
                finally
                {
                    Hats.ReaderWriterLock?.ExitWriteLock();
                }
            }
        }
    }
}
