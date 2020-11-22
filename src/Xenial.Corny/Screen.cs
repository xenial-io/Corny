using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace Xenial.Delicious.Corny
{
    public class Screen :
        IRenderable,
        ISupportKeyPress,
        ISupportMouseClick
    {
        public List<IRenderable> Renderables { get; } = new List<IRenderable>();

        private static readonly object locker = new object();

        public int Width { get; private set; } = ConsoleWidth;

        private static int ConsoleWidth => Math.Min(Console.WindowWidth, Console.LargestWindowWidth);

        private static int ConsoleHeight => Math.Min(Console.WindowHeight, Console.LargestWindowHeight);

        public int Height { get; private set; } = ConsoleHeight;

        private readonly Thread keyThread;

        private readonly ThreadStart keyThreadStart;

        private readonly Thread resizeThread;

        private readonly ThreadStart resizeThreadStart;

        private readonly Thread mouseThread;

        private readonly ThreadStart mouseThreadStart;

        private readonly string name;

        public bool MouseSupportEnabled { get; set; } = true;

        public Screen()
        {
            keyThreadStart = new ThreadStart(LoopForKeypress);
            keyThread = new Thread(keyThreadStart);

            resizeThreadStart = new ThreadStart(LoopForResize);
            resizeThread = new Thread(resizeThreadStart);

            mouseThreadStart = new ThreadStart(LoopForMouse);
            mouseThread = new Thread(mouseThreadStart);
            name = string.Empty; //Check what name is used for
        }

        public void SetSize(int height, int width)
        {
            lock (locker)
            {
                Height = Math.Min(height, Console.LargestWindowHeight);
                Width = Math.Min(width, Console.LargestWindowWidth);
                Render();
            }
        }

        public void Fullscreen() => SetSize(Console.LargestWindowHeight, Console.LargestWindowWidth);

        public void Run()
        {
            Render();

            if (resizeThread.Name == null)
            {
                resizeThread.Name = "Refresh loop for " + name;
                resizeThread.Start();
            }

            if (MouseSupportEnabled)
            {
                if (mouseThread.Name == null)
                {
                    mouseThread.Name = "Mouse loop for " + name;
                    mouseThread.Start();
                }
            }
            else
            {
                if (keyThread.Name == null)
                {
                    keyThread.Name = "Keypress loop for " + name;
                    keyThread.Start();
                }
            }
        }

        void ISupportKeyPress.OnKeyPress(KeyPressEventArgs args) => OnKeyPressCore(args);

        protected virtual void OnKeyPressCore(KeyPressEventArgs args)
        {
            if (args == null || args.Cancel)
            {
                return;
            }

            KeyPress?.Invoke(this, args);

            if (args.Cancel)
            {
                return;
            }

            foreach (var keyPress in Renderables.OfType<ISupportKeyPress>())
            {
                keyPress.OnKeyPress(args);
                if (args.Cancel)
                {
                    return;
                }
            }

            if (RefreshKeyHit(args))
            {
                return;
            }
        }

        void ISupportMouseClick.OnMouseClick(MouseClickEventArgs args) => OnMouseClickCore(args);

        protected virtual void OnMouseClickCore(MouseClickEventArgs args)
        {
            if (args == null || args.Cancel)
            {
                return;
            }

            MouseClick?.Invoke(this, args);

            if (args.Cancel)
            {
                return;
            }

            foreach (var mouseClick in Renderables.OfType<ISupportMouseClick>())
            {
                mouseClick.OnMouseClick(args);
                if (args.Cancel)
                {
                    return;
                }
            }
        }

        protected virtual bool RefreshKeyHit(KeyPressEventArgs args)
        {
            if (args == null)
            {
                return false;
            }

            if (args.KeyInfo.Key == ConsoleKey.F5)
            {
                Render();
                return true;
            }

            return false;
        }

        public event EventHandler<KeyPressEventArgs>? KeyPress;
        public event EventHandler<MouseClickEventArgs>? MouseClick;

        private void LoopForKeypress()
        {
            while (true)
            {
                // Blocks on the next function call.
                var cki = Console.ReadKey(true);

                ((ISupportKeyPress)this).OnKeyPress(new KeyPressEventArgs(cki));
            }
        }

        private void LoopForResize()
        {
            while (true)
            {
                try
                {
                    // Read and write the buffer size/location and window size/location and cursor position,
                    // but be aware it will be rudely interrupted if the console is resized by the user.
                    Thread.Sleep(500);
                    if (Width != ConsoleWidth || Height != ConsoleHeight)
                    {
                        lock (locker)
                        {
                            Width = ConsoleWidth;
                            Height = ConsoleHeight;
                            Render();
                        }
                    }
                }
                catch (IOException) { }
                catch (ArgumentOutOfRangeException) { }
            }
        }

        private const short altVKCode = 0x12;

        private void LoopForMouse()
        {
            var handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);

            var mode = 0;

            if (!NativeMethods.GetConsoleMode(handle, ref mode))
            {
                throw new Win32Exception();
            }

            mode |= NativeMethods.ENABLE_MOUSE_INPUT;
            mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;
            mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;

            if (!NativeMethods.SetConsoleMode(handle, mode))
            {
                throw new Win32Exception();
            }

            uint recordLen = 0;
            bool r;

            var numEventsRead = -1;

            lock (ReadKeySyncObject)
            {
                while (true)
                {

                    r = NativeMethods.ReadConsoleInput(handle, out var record, 1, ref recordLen);
                    if (!r)
                    {
                        throw new Win32Exception();
                    }

                    switch (record.EventType)
                    {
                        case NativeMethods.MOUSE_EVENT:
                            {
                                switch (record.MouseEvent.DwEventFlags)
                                {
                                    case NativeMethods.MOUSE_EVENT_RECORD.MOUSE_MOVED:
                                        break;

                                    case NativeMethods.MOUSE_EVENT_RECORD.MOUSE_WHEELED:
                                        break;

                                    case NativeMethods.MOUSE_EVENT_RECORD.MOUSE_HWHEELED:
                                        break;

                                    case NativeMethods.MOUSE_EVENT_RECORD.DOUBLE_CLICK:
                                        break;
                                    case 0:

                                        var button = MouseButton.Left;

                                        if (record.MouseEvent.DwButtonState == NativeMethods.MOUSE_EVENT_RECORD.FROM_LEFT_1ST_BUTTON_PRESSED)
                                        {
                                            button = MouseButton.Left;
                                        }

                                        if (record.MouseEvent.DwButtonState == NativeMethods.MOUSE_EVENT_RECORD.FROM_LEFT_2ND_BUTTON_PRESSED)
                                        {
                                            button = MouseButton.Middle;
                                        }
                                        if (record.MouseEvent.DwButtonState == NativeMethods.MOUSE_EVENT_RECORD.RIGHTMOST_BUTTON_PRESSED)
                                        {
                                            button = MouseButton.Right;
                                        }

                                        ((ISupportMouseClick)this).OnMouseClick(new MouseClickEventArgs(
                                            new Point(record.MouseEvent.DwMousePosition.X, record.MouseEvent.DwMousePosition.Y),
                                            button,
                                            new ConsoleKeyInfo(Convert.ToChar(record.KeyEvent.WVirtualKeyCode),
                                            (ConsoleKey)record.KeyEvent.WVirtualKeyCode,
                                            (record.MouseEvent.DwControlKeyState & NativeMethods.MOUSE_EVENT_RECORD.SHIFT_PRESSED) != 0,
                                            (record.MouseEvent.DwControlKeyState & NativeMethods.MOUSE_EVENT_RECORD.RIGHT_ALT_PRESSED) != 0
                                                || (record.MouseEvent.DwControlKeyState & NativeMethods.MOUSE_EVENT_RECORD.LEFT_ALT_PRESSED) != 0,
                                            (record.MouseEvent.DwControlKeyState & NativeMethods.MOUSE_EVENT_RECORD.LEFT_CTRL_PRESSED) != 0
                                                || (record.MouseEvent.DwControlKeyState & NativeMethods.MOUSE_EVENT_RECORD.RIGHT_CTRL_PRESSED) != 0)));

                                        break;
                                    default:
                                        Debug.WriteLine(string.Format("    dwEventFlags ....: 0x{0:X4}  ", record.MouseEvent.DwEventFlags));
                                        break;
                                }
                            }
                            break;

                        case NativeMethods.KEY_EVENT:
                            {
                                if (cachedInputRecord.EventType == NativeMethods.KEY_EVENT)
                                {
                                    // We had a previous keystroke with repeated characters.
                                    record = cachedInputRecord;
                                    if (cachedInputRecord.KeyEvent.WRepeatCount == 0)
                                    {
                                        cachedInputRecord.EventType = -1;
                                    }
                                    else
                                    {
                                        cachedInputRecord.KeyEvent.WRepeatCount--;
                                    }
                                    // We will return one key from this method, so we decrement the
                                    // repeatCount here, leaving the cachedInputRecord in the "queue".

                                }
                                else
                                { // We did NOT have a previous keystroke with repeated characters:


                                    if (!r || numEventsRead == 0)
                                    {
                                        // This will fail when stdin is redirected from a file or pipe. 
                                        // We could theoretically call Console.Read here, but I 
                                        // think we might do some things incorrectly then.
                                        throw new InvalidOperationException("InvalidOperation_ConsoleReadKeyOnFile");
                                    }

                                    var keyCode = record.KeyEvent.WVirtualKeyCode;

                                    // First check for non-keyboard events & discard them. Generally we tap into only KeyDown events and ignore the KeyUp events
                                    // but it is possible that we are dealing with a Alt+NumPad unicode key sequence, the final unicode char is revealed only when 
                                    // the Alt key is released (i.e when the sequence is complete). To avoid noise, when the Alt key is down, we should eat up 
                                    // any intermediate key strokes (from NumPad) that collectively forms the Unicode character.  

                                    if (!IsKeyDownEvent(record) && keyCode != altVKCode)
                                    {
                                        continue;
                                    }

                                    var ch = record.KeyEvent.UnicodeChar;

                                    // In a Alt+NumPad unicode sequence, when the alt key is released uChar will represent the final unicode character, we need to 
                                    // surface this. VirtualKeyCode for this event will be Alt from the Alt-Up key event. This is probably not the right code, 
                                    // especially when we don't expose ConsoleKey.Alt, so this will end up being the hex value (0x12). VK_PACKET comes very 
                                    // close to being useful and something that we could look into using for this purpose... 

                                    if (ch == 0 && IsModKey(record))
                                    {
                                        continue;
                                    }

                                    // When Alt is down, it is possible that we are in the middle of a Alt+NumPad unicode sequence.
                                    // Escape any intermediate NumPad keys whether NumLock is on or not (notepad behavior)
                                    var key = (ConsoleKey)keyCode;

                                    if (IsAltKeyDown(record) && (key >= ConsoleKey.NumPad0 && key <= ConsoleKey.NumPad9
                                                            || key == ConsoleKey.Clear || key == ConsoleKey.Insert
                                                            || key >= ConsoleKey.PageUp && key <= ConsoleKey.DownArrow))
                                    {
                                        continue;
                                    }

                                    if (record.KeyEvent.WRepeatCount > 1)
                                    {
                                        record.KeyEvent.WRepeatCount--;
                                        cachedInputRecord = record;
                                    }
                                }  // we did NOT have a previous keystroke with repeated characters.


                                var state = (ControlKeyState)record.KeyEvent.DwControlKeyState;
                                var shift = (state & ControlKeyState.ShiftPressed) != 0;
                                var alt = (state & (ControlKeyState.LeftAltPressed | ControlKeyState.RightAltPressed)) != 0;
                                var control = (state & (ControlKeyState.LeftCtrlPressed | ControlKeyState.RightCtrlPressed)) != 0;

                                var info = new ConsoleKeyInfo(record.KeyEvent.UnicodeChar, (ConsoleKey)record.KeyEvent.WVirtualKeyCode, shift, alt, control);

                                ((ISupportKeyPress)this).OnKeyPress(new KeyPressEventArgs(info));
                            }
                            break;
                    }
                }
            }
        }

        [Flags]
        internal enum ControlKeyState
        {
            RightAltPressed = 0x0001,
            LeftAltPressed = 0x0002,
            RightCtrlPressed = 0x0004,
            LeftCtrlPressed = 0x0008,
            ShiftPressed = 0x0010,
            NumLockOn = 0x0020,
            ScrollLockOn = 0x0040,
            CapsLockOn = 0x0080,
            EnhancedKey = 0x0100
        }

        // For tracking Alt+NumPad unicode key sequence. When you press Alt key down 
        // and press a numpad unicode decimal sequence and then release Alt key, the
        // desired effect is to translate the sequence into one Unicode KeyPress. 
        // We need to keep track of the Alt+NumPad sequence and surface the final
        // unicode char alone when the Alt key is released. 
        [SecurityCritical]  // auto-generated
        private static bool IsAltKeyDown(NativeMethods.INPUT_RECORD ir)
            => ((ControlKeyState)ir.KeyEvent.DwControlKeyState
                & (ControlKeyState.LeftAltPressed | ControlKeyState.RightAltPressed)) != 0;

        [SecurityCritical]  // auto-generated
        private static bool IsModKey(NativeMethods.INPUT_RECORD ir)
        {
            // We should also skip over Shift, Control, and Alt, as well as caps lock.
            // Apparently we don't need to check for 0xA0 through 0xA5, which are keys like 
            // Left Control & Right Control. See the ConsoleKey enum for these values.
            var keyCode = ir.KeyEvent.WVirtualKeyCode;
            return keyCode >= 0x10 && keyCode <= 0x12
                    || keyCode == 0x14 || keyCode == 0x90 || keyCode == 0x91;
        }


        // Skip non key events. Generally we want to surface only KeyDown event 
        // and suppress KeyUp event from the same Key press but there are cases
        // where the assumption of KeyDown-KeyUp pairing for a given key press 
        // is invalid. For example in IME Unicode keyboard input, we often see
        // only KeyUp until the key is released.  
        [SecurityCritical]  // auto-generated
        private static bool IsKeyDownEvent(NativeMethods.INPUT_RECORD ir)
            => ir.EventType == NativeMethods.KEY_EVENT && ir.KeyEvent.BKeyDown;

        // ReadLine & Read can't use this because they need to use ReadFile
        // to be able to handle redirected input.  We have to accept that
        // we will lose repeated keystrokes when someone switches from
        // calling ReadKey to calling Read or ReadLine.  Those methods should 
        // ideally flush this cache as well.
        [SecurityCritical] // auto-generated
        private NativeMethods.INPUT_RECORD cachedInputRecord;

        // Use this for blocking in Console.ReadKey, which needs to protect itself in case multiple threads call it simultaneously.
        // Use a ReadKey-specific lock though, to allow other fields to be initialized on this type.
        private static volatile object? readKeySyncObject;
        private static object ReadKeySyncObject
        {
            get
            {
                Contract.Ensures(Contract.Result<object>() != null);
                if (readKeySyncObject == null)
                {
                    var o = new object();
#pragma warning disable 0420
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    Interlocked.CompareExchange<object>(ref readKeySyncObject, o, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore 0420
                }
                return readKeySyncObject;
            }
        }

        private Point consoleLocation;
        private ConsoleColor foreColor;
        private ConsoleColor backColor;

        public void Render()
        {
            bool IsCursorVisible()
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return Console.CursorVisible;
                }
                return true;
            }

            void SetCursorVisible(bool cursorVisible)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Console.CursorVisible = cursorVisible;
                    Console.CursorSize = cursorVisible ? 100 : 1;
                }
            }

            lock (locker)
            {
                var cursorVisible = IsCursorVisible();
                SetCursorVisible(false);

                consoleLocation = new Point(Console.CursorLeft, Console.CursorTop);
                foreColor = Console.ForegroundColor;
                backColor = Console.BackgroundColor;

                try
                {
                    Console.ResetColor();

                    Console.Clear();

                    if (!string.IsNullOrEmpty(name))
                    {
                        Console.Title = name;
                    }

                    // Resize the window and the buffer to the form's size.
                    if (Console.BufferHeight != Height || Console.BufferWidth != Width)
                    {
                        Console.SetWindowSize(Width, Height);
                        Console.SetBufferSize(Width, Height);
                    }

                    if (Console.WindowHeight != Height || Console.WindowWidth != Width)
                    {
                        Console.SetBufferSize(Width, Height);
                        Console.SetWindowSize(Width, Height);
                    }

                    foreach (var renderable in Renderables)
                    {
                        renderable.Render();
                    }
                }
                finally
                {
                    Console.ForegroundColor = foreColor;
                    Console.BackgroundColor = backColor;

                    Console.SetCursorPosition(consoleLocation.X, consoleLocation.Y);

                    SetCursorVisible(cursorVisible);
                }
            }
        }

    }
}
