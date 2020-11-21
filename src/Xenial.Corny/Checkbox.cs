using System;
using System.Drawing;
using System.Linq;

using Xenial.Delicious.Corny.Drawing;

namespace Xenial.Delicious.Corny
{
    public class Checkbox : Control, IControl, ISupportKeyPress
    {
        public event EventHandler<KeyPressEventArgs>? KeyPress;

        bool IControl.SupportsFocus => true;

        private bool @checked;
        public bool Checked
        {
            get => @checked;
            set
            {
                @checked = value;
                OnCheckedChanged();
            }
        }

        protected virtual void OnCheckedChanged()
            => CheckedChanged?.Invoke(this, EventArgs.Empty);

        public string? Text { get; set; }

        public event EventHandler? CheckedChanged;

        public override void Render()
        {
            new TextPart
            {
                BackColor = BackColor,
                ForeColor = ForeColor,
                Location = Position,
                Text = $"[{(Checked ? 'X' : ' ')}]"
            }.Render();

            if (!string.IsNullOrEmpty(Text))
            {
                new TextPart
                {
                    BackColor = ConsoleColor.Black,
                    ForeColor = ConsoleColor.DarkGray,
                    Location = new Point(Position.X + 3, Position.Y),
                    Text = $" {Text}"
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

            if (SpaceKeyHit(args))
            {
                return;
            }
        }

        protected virtual bool SpaceKeyHit(KeyPressEventArgs args)
        {
            if (args == null)
            {
                return false;
            }

            if (args.KeyInfo.Key == ConsoleKey.Spacebar)
            {
                args.Cancel = true;
                Checked = !Checked;
                Render();
                Focus();
                return true;
            }
            return false;
        }

        public override void Focus()
        {
            if (((IControl)this).SupportsFocus)
            {
                if (Parent != null)
                {
                    Parent.FocusedControl = this;
                }

                SetCursorPosition(new Point(Position.X + 1, Position.Y));
            }
        }
    }
}
