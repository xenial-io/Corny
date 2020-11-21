using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Xenial.Delicious.Corny.Drawing;

namespace Xenial.Delicious.Corny
{
    public class Textbox : Control,
        IControl,
        ISupportKeyPress,
        ISupportMouseClick,
        INotifyPropertyChanged
    {
        public event EventHandler<KeyPressEventArgs>? KeyPress;
        public event EventHandler<MouseClickEventArgs>? MouseClick;

        public int Width { get; set; }
        public bool IsPassword { get; set; }

        IList<IControl> IControl.Controls => throw new NotImplementedException();

        public bool SupportsFocus => true;

        IControl? IControl.FocusedControl
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        private string? text;
        public string? Text
        {
            get => text;
            set
            {
                text = value;
                OnTextChanged();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
            }
        }

        protected virtual void OnTextChanged()
        {
            Render();
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnMouseClick()
        {
            Render();
            //TODO: mouse click
            MouseClick?.Invoke(this, new MouseClickEventArgs(default, default, default));
        }

        public event EventHandler? TextChanged;

        public override void Render()
        {
            Console.ForegroundColor = ForeColor;

            new Line
            {
                Colour = BackColor,
                Location = Position,
                Orientation = Orientation.Horizontal,
                Length = Width
            }.Render();

            new TextPart
            {
                ForeColor = ForeColor,
                BackColor = BackColor,
                Text = IsPassword ? new string('*', Text?.Length ?? 0) : Text,
                Location = Position
            }.Render();
        }

        private int cursorPosition;
        public int CursorPosition
        {
            get => cursorPosition;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (value >= Width)
                {
                    value = Width;
                }

                if (Text == null || Text.Length == 0)
                {
                    value = 0;
                }

                if (Text != null && value >= Text.Length)
                {
                    value = Text.Length;
                }

                cursorPosition = value;
                OnCursorPositionChanged();
            }
        }

        protected virtual void OnCursorPositionChanged()
        {
            CursorPositionChanged?.Invoke(this, EventArgs.Empty);
            SetCursorPosition();
        }

        protected override void SetCursorPosition() => SetCursorPosition(new Point(Position.X + CursorPosition, Position.Y));

        public event EventHandler? CursorPositionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

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

            if (EscKeyOrShiftTabHit(args))
            {
                return;
            }

            if (EnterOrTabKeyHit(args))
            {
                return;
            }

            if (LeftOrRightKeyHit(args))
            {
                return;
            }

            if (HomeOrEndKeyHit(args))
            {
                return;
            }

            if (BackspaceKeyHit(args))
            {
                return;
            }

            if (EntfKeyHit(args))
            {
                return;
            }

            if (AlphaNumericHit(args))
            {
                return;
            }
        }

        protected virtual bool EnterOrTabKeyHit(KeyPressEventArgs args)
        {
            if (args == null)
            {
                return false;
            }

            if ((args.KeyInfo.Key == ConsoleKey.Enter || args.KeyInfo.Key == ConsoleKey.Tab) && Parent != null)
            {
                Parent.SelectNextControl();

                args.Cancel = true;

                return true;
            }

            return false;
        }

        protected virtual bool EscKeyOrShiftTabHit(KeyPressEventArgs args)
        {
            if (args == null)
            {
                return false;
            }

            if ((args.KeyInfo.Key == ConsoleKey.Escape
                || args.KeyInfo.Modifiers == ConsoleModifiers.Shift
                && args.KeyInfo.Key == ConsoleKey.Tab)
                && Parent != null)
            {
                Parent.SelectPreviousControl();

                args.Cancel = true;

                return true;
            }

            return false;
        }

        protected virtual bool HomeOrEndKeyHit(KeyPressEventArgs args)
        {
            if (args == null)
            {
                return false;
            }

            if (args.KeyInfo.Key == ConsoleKey.Home || args.KeyInfo.Key == ConsoleKey.End)
            {
                if (args.KeyInfo.Key == ConsoleKey.Home)
                {
                    CursorPosition = 0;
                }

                if (args.KeyInfo.Key == ConsoleKey.End)
                {
                    CursorPosition = int.MaxValue;
                }

                args.Cancel = true;

                return true;
            }

            return false;
        }

        protected virtual bool LeftOrRightKeyHit(KeyPressEventArgs args)
        {
            if (args == null)
            {
                return false;
            }

            if (args.KeyInfo.Key == ConsoleKey.LeftArrow || args.KeyInfo.Key == ConsoleKey.RightArrow)
            {
                if (args.KeyInfo.Key == ConsoleKey.LeftArrow)
                {
                    CursorPosition--;
                }

                if (args.KeyInfo.Key == ConsoleKey.RightArrow)
                {
                    CursorPosition++;
                }

                args.Cancel = true;

                return true;
            }

            return false;
        }

        protected virtual bool BackspaceKeyHit(KeyPressEventArgs args)
        {
            if (args == null)
            {
                return false;
            }

            if (args.KeyInfo.Key == ConsoleKey.Backspace)
            {
                var cursorPosition = CursorPosition;

                Text = new string(Text?.Take(CursorPosition - 1).ToArray()) + new string(Text?.Skip(CursorPosition).ToArray());

                CursorPosition = cursorPosition - 1;

                args.Cancel = true;

                return true;
            }

            return false;
        }

        protected virtual bool EntfKeyHit(KeyPressEventArgs args)
        {
            if (args == null)
            {
                return false;
            }

            if (args.KeyInfo.Key == ConsoleKey.Delete)
            {
                var cursorPosition = CursorPosition;

                Text = new string(Text?.Take(CursorPosition).ToArray()) + new string(Text?.Skip(CursorPosition + 1).ToArray());

                CursorPosition = cursorPosition;

                args.Cancel = true;

                return true;
            }

            return false;
        }

        protected virtual bool AlphaNumericHit(KeyPressEventArgs args)
        {
            if (args == null)
            {
                return false;
            }

            // A key's been pressed.  Figure out what to do.
            // All actions will be against the current field, stored in _field.
            var cChar = args.KeyInfo.KeyChar;

            if (cChar != 0)
            {
                var before = new string(Text?.Take(CursorPosition).ToArray());
                var after = new string(Text?.Skip(CursorPosition).ToArray());
                var newText = before + cChar + after;

                Text = newText;

                CursorPosition++;

                args.Cancel = true;

                return true;
            }

            return false;
        }

        void ISupportMouseClick.OnMouseClick(MouseClickEventArgs args) => OnMouseClickCore(args);

        protected virtual void OnMouseClickCore(MouseClickEventArgs args)
        {
            if (args == null || args.Cancel)
            {
                return;
            }

            if (args.Location.Y == Position.Y
                && args.Location.X >= Position.X
                && args.Location.X < Position.X + Width)
            {
                args.Cancel = true;
                Focus();
            }
        }
    }
}
