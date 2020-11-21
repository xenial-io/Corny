using System;
using System.Drawing;
using System.Linq;

using Xenial.Delicious.Corny.Drawing;

namespace Xenial.Delicious.Corny
{
    public class Button : Control, IControl, ISupportKeyPress
    {
        public event EventHandler<KeyPressEventArgs>? KeyPress;

        public event EventHandler? Execute;

        bool IControl.SupportsFocus => true;

        public int Width { get; set; }

        public string? Text { get; set; }

        public Button()
        {
            ForeColor = ConsoleColor.White;
            BackColor = ConsoleColor.Red;
            Width = 1;
        }

        public override void Render()
        {
            if (!((IControl)this).IsFocused)
            {
                new TextPart
                {
                    BackColor = ConsoleColor.Black,
                    ForeColor = BackColor,
                    Location = Position,
                    Text = $"┌{new string('─', Width - 2)}┐"
                }.Render();

                new TextPart
                {
                    BackColor = ConsoleColor.Black,
                    ForeColor = BackColor,
                    Location = new Point(Position.X, Position.Y + 1),
                    Text = "│"
                }.Render();
            }
            else
            {
                new TextPart
                {
                    BackColor = ConsoleColor.Black,
                    ForeColor = BackColor,
                    Location = Position,
                    Text = $"╔{new string('═', Width - 2)}╗"
                }.Render();

                new TextPart
                {
                    BackColor = ConsoleColor.Black,
                    ForeColor = BackColor,
                    Location = new Point(Position.X, Position.Y + 1),
                    Text = "║"
                }.Render();
            }

            new TextPart
            {
                BackColor = ConsoleColor.Black,
                ForeColor = ForeColor,
                Location = new Point(Position.X + 2, Position.Y + 1),
                Text = Text
            }.Render();

            if (!((IControl)this).IsFocused)
            {
                new TextPart
                {
                    BackColor = ConsoleColor.Black,
                    ForeColor = BackColor,
                    Location = new Point(Position.X + Width - 1, Position.Y + 1),
                    Text = "│"
                }.Render();

                new TextPart
                {
                    BackColor = ConsoleColor.Black,
                    ForeColor = BackColor,
                    Location = new Point(Position.X, Position.Y + 2),
                    Text = $"└{new string('─', Width - 2)}┘"
                }.Render();
            }
            else
            {
                new TextPart
                {
                    BackColor = ConsoleColor.Black,
                    ForeColor = BackColor,
                    Location = new Point(Position.X + Width - 1, Position.Y + 1),
                    Text = "║"
                }.Render();

                new TextPart
                {
                    BackColor = ConsoleColor.Black,
                    ForeColor = BackColor,
                    Location = new Point(Position.X, Position.Y + 2),
                    Text = $"╚{new string('═', Width - 2)}╝"
                }.Render();
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

            if (EnterOrSpaceKeyHit(args))
            {
                return;
            }
        }

        protected virtual bool EnterOrSpaceKeyHit(KeyPressEventArgs args)
        {
            if (args == null)
            {
                return false;
            }

            if (args.KeyInfo.Key == ConsoleKey.Spacebar || args.KeyInfo.Key == ConsoleKey.Enter)
            {
                Execute?.Invoke(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        public override void Focus()
        {
            ((IControl)this).IsFocused = false;

            if (((IControl)this).SupportsFocus && Parent != null)
            {
                Parent.FocusedControl = this;

                SetCursorPosition(new Point(Location.X + 1, Location.Y));

                ((IControl)this).IsFocused = true;
            }
        }

        protected override void OnFocusedChanged()
        {
            Console.CursorVisible = false;
            Render();
        }
    }
}
