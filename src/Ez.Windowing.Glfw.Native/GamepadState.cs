using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Ez.Windowing.GLFW.Native
{
    /// <summary>
    /// This describes the input state of a gamepad.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct GamepadState
    {
#pragma warning disable IDE0044
        private byte _00;
        private byte _01;
        private byte _02;
        private byte _03;
        private byte _04;
        private byte _05;
        private byte _06;
        private byte _07;
        private byte _08;
        private byte _09;
        private byte _0A;
        private byte _0B;
        private byte _0C;
        private byte _0D;

        private float _0F;
        private float _10;
        private float _11;
        private float _12;
        private float _13;
        private float _14;
        private float _15;
        private float _16;
        private float _17;
        private float _18;
        private float _19;
        private float _1A;
        private float _1B;
        private float _1C;
        private float _1D;
#pragma warning restore IDE0044

        public byte GetButton(int index) => 
            Unsafe.Add(ref _00, index);

        public void SetButton(int index, byte value) => 
            Unsafe.Add(ref _00, index) = value;

        public float GetAxes(int index) =>
            Unsafe.Add(ref _0F, index);

        public float SetAxes(int index, float value) =>
            Unsafe.Add(ref _0F, index) = value;
    }
}
