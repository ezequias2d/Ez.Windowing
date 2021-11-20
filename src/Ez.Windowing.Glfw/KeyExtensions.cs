using Ez.Windowing.GLFW.Native.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ez.Windowing.GLFW.Native
{
    internal static class KeyExtensions
    {
        public static Key ToKey(this Keys key)
        {
            return key switch
            {
                Keys.A => Key.A,
                Keys.Apostrophe => Key.Apostrophe,
                Keys.B => Key.B,
                Keys.Backslash => Key.Backslash,
                Keys.Backspace => Key.Backspace,
                Keys.C => Key.C,
                Keys.CapsLock => Key.CapsLock,
                Keys.Comma => Key.Comma,
                Keys.D => Key.D,
                Keys.D0 => Key.D0,
                Keys.D1 => Key.D1,
                Keys.D2 => Key.D2,
                Keys.D3 => Key.D3,
                Keys.D4 => Key.D4,
                Keys.D5 => Key.D5,
                Keys.D6 => Key.D6,
                Keys.D7 => Key.D7,
                Keys.D8 => Key.D8,
                Keys.D9 => Key.D9,
                Keys.Delete => Key.Delete,
                Keys.Down => Key.Down,
                Keys.E => Key.E,
                Keys.End => Key.End,
                Keys.Enter => Key.Enter,
                Keys.Equal => Key.Equal,
                Keys.Escape => Key.Escape,
                Keys.F => Key.F,
                Keys.F1 => Key.F1,
                Keys.F10 => Key.F10,
                Keys.F11 => Key.F11,
                Keys.F12 => Key.F12,
                Keys.F13 => Key.F13,
                Keys.F14 => Key.F14,
                Keys.F15 => Key.F15,
                Keys.F16 => Key.F16,
                Keys.F17 => Key.F17,
                Keys.F18 => Key.F18,
                Keys.F19 => Key.F19,
                Keys.F2 => Key.F2,
                Keys.F20 => Key.F20,
                Keys.F21 => Key.F21,
                Keys.F22 => Key.F22,
                Keys.F23 => Key.F23,
                Keys.F24 => Key.F24,
                Keys.F25 => Key.F25,
                Keys.F3 => Key.F3,
                Keys.F4 => Key.F4,
                Keys.F5 => Key.F5,
                Keys.F6 => Key.F6,
                Keys.F7 => Key.F7,
                Keys.F8 => Key.F8,
                Keys.F9 => Key.F9,
                Keys.G => Key.G,
                Keys.GraveAccent => Key.GraveAccent,
                Keys.H => Key.H,
                Keys.Home => Key.Home,
                Keys.I => Key.I,
                Keys.Insert => Key.Insert,
                Keys.J => Key.J,
                Keys.K => Key.K,
                Keys.KeyPad0 => Key.KeyPad0,
                Keys.KeyPad1 => Key.KeyPad1,
                Keys.KeyPad2 => Key.KeyPad2,
                Keys.KeyPad3 => Key.KeyPad3,
                Keys.KeyPad4 => Key.KeyPad4,
                Keys.KeyPad5 => Key.KeyPad5,
                Keys.KeyPad6 => Key.KeyPad6,
                Keys.KeyPad7 => Key.KeyPad7,
                Keys.KeyPad8 => Key.KeyPad8,
                Keys.KeyPad9 => Key.KeyPad9,
                Keys.KeyPadAdd => Key.KeyPadAdd,
                Keys.KeyPadDecimal => Key.KeyPadDecimal,
                Keys.KeyPadDivide => Key.KeyPadDivide,
                Keys.KeyPadEnter => Key.KeyPadEnter,
                Keys.KeyPadEqual => Key.KeyPadEqual,
                Keys.KeyPadMultiply => Key.KeyPadMultiply,
                Keys.KeyPadSubtract => Key.KeyPadSubtract,
                Keys.L => Key.L,
                Keys.Left => Key.Left,
                Keys.LeftAlt => Key.LeftAlt,
                Keys.LeftBracket => Key.LeftBracket,
                Keys.LeftControl => Key.LeftControl,
                Keys.LeftShift => Key.LeftShift,
                Keys.LeftSuper => Key.LeftSuper,
                Keys.M => Key.M,
                Keys.Menu => Key.Menu,
                Keys.Minus => Key.Minus,
                Keys.N => Key.N,
                Keys.NumLock => Key.NumLock,
                Keys.O => Key.O,
                Keys.P => Key.P,
                Keys.PageDown => Key.PageDown,
                Keys.PageUp => Key.PageUp,
                Keys.Pause => Key.Pause,
                Keys.Period => Key.Period,
                Keys.PrintScreen => Key.PrintScreen,
                Keys.Q => Key.Q,
                Keys.R => Key.R,
                Keys.Right => Key.Right,
                Keys.RightAlt => Key.RightAlt,
                Keys.RightBracket => Key.RightBracket,
                Keys.RightControl => Key.RightControl,
                Keys.RightShift => Key.RightShift,
                Keys.RightSuper => Key.RightSuper,
                Keys.S => Key.S,
                Keys.ScrollLock => Key.ScrollLock,
                Keys.Semicolon => Key.Semicolon,
                Keys.Slash => Key.Slash,
                Keys.Space => Key.Space,
                Keys.T => Key.T,
                Keys.Tab => Key.Tab,
                Keys.U => Key.U,
                Keys.Unknown => Key.Unknown,
                Keys.Up => Key.Up,
                Keys.V => Key.V,
                Keys.W => Key.W,
                Keys.X => Key.X,
                Keys.Y => Key.Y,
                Keys.Z => Key.Z,
                Keys.World1 => Key.LastKey + 1,
                Keys.World2 => Key.LastKey + 2,
                _ => throw new ArgumentOutOfRangeException(nameof(key), $"{nameof(key)}: {key}"),
            };
        }

        public static KeyModifiers ToKeyModifiers(this Enums.KeyModifiers modifiers)
        {
            KeyModifiers output = KeyModifiers.None;

            if (modifiers.HasFlag(Enums.KeyModifiers.Alt))
                output |= KeyModifiers.Alt;

            if (modifiers.HasFlag(Enums.KeyModifiers.CapsLock))
                output |= KeyModifiers.CapsLock;

            if (modifiers.HasFlag(Enums.KeyModifiers.Control))
                output |= KeyModifiers.Control;

            if (modifiers.HasFlag(Enums.KeyModifiers.NumLock))
                output |= KeyModifiers.NumLock;

            if (modifiers.HasFlag(Enums.KeyModifiers.Shift))
                output |= KeyModifiers.Shift;

            if (modifiers.HasFlag(Enums.KeyModifiers.Super))
                output |= KeyModifiers.Super;

            return output;
        }

        public static KeyEvent ToKeyEvent(this InputAction inputAction)
        {
            return inputAction switch
            {
                InputAction.Press => KeyEvent.Down,
                InputAction.Release => KeyEvent.Up,
                InputAction.Repeat => KeyEvent.Repeat,
                _ => throw new ArgumentOutOfRangeException(nameof(inputAction), $"{nameof(inputAction)}: {inputAction}"),
            };
        }

        public static MouseButton ToMouseButton(this Enums.MouseButton mouseButton)
        {
            return mouseButton switch
            {
                Enums.MouseButton.Button1 => MouseButton.Button1,
                Enums.MouseButton.Button2 => MouseButton.Button2,
                Enums.MouseButton.Button3 => MouseButton.Button3,
                Enums.MouseButton.Button4 => MouseButton.Button4,
                Enums.MouseButton.Button5 => MouseButton.Button5,
                Enums.MouseButton.Button6 => MouseButton.Button6,
                Enums.MouseButton.Button7 => MouseButton.Button7,
                Enums.MouseButton.Button8 => MouseButton.Button8,
                _ => MouseButton.None,
            };
        }

        public static CursorMode ToCursorMode(this CursorModeValue value)
        {
            return value switch
            {
                CursorModeValue.CursorDisabled => CursorMode.Disabled,
                CursorModeValue.CursorHidden => CursorMode.Hidden,
                CursorModeValue.CursorNormal => CursorMode.Visible,
                _ => throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)}: {value}"),
            };
        }
    }
}
