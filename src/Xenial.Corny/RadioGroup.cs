using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Xenial.Delicious.Corny.Drawing;

namespace Xenial.Delicious.Corny
{
    public class RadioGroup : Control, IRenderable, IControl, ISupportKeyPress
    {
        bool IControl.SupportsFocus => true;

        public event EventHandler<KeyPressEventArgs>? KeyPress;

        public IList<string> Options { get; } = new List<string>();

        private string? @checked;
        public string? Checked
        {
            get => @checked;
            set
            {
                @checked = value;
                OnCheckedChanged();
            }
        }

        protected virtual void OnCheckedChanged()
        {
            SelectedIndex = string.IsNullOrEmpty(Checked) ? 0 : Options.IndexOf(Checked!);
            Render();
            Focus();
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? CheckedChanged;

        private int selectedIndex;
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (value <= 0)
                {
                    value = 0;
                }

                if (value >= Options.Count - 1)
                {
                    value = Options.Count - 1;
                }

                selectedIndex = value;
                OnSelectedIndexChanged();
            }
        }

        protected virtual void OnSelectedIndexChanged()
        {
            Render();
            Focus();
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? SelectedIndexChanged;

        public override void Render()
        {
            var i = 0;

            foreach (var option in Options)
            {
                var position = new Point(Position.X, Position.Y + i);
                new TextPart
                {
                    BackColor = BackColor,
                    ForeColor = ForeColor,
                    Location = position,
                    Text = $"({(Checked == option ? 'O' : ' ')})"
                }.Render();

                new TextPart
                {
                    BackColor = ConsoleColor.Black,
                    ForeColor = ConsoleColor.DarkGray,
                    Location = new Point(position.X + 3, position.Y),
                    Text = $" {option}"
                }.Render();

                i++;
            }
        }

        void ISupportKeyPress.OnKeyPress(KeyPressEventArgs args) => OnKeyPressCore(args);

        protected virtual void OnKeyPressCore(KeyPressEventArgs args)
        {
            if (args == null || args.Cancel) { return; }

            KeyPress?.Invoke(this, args);

            if (args.Cancel) { return; }

            if (UpOrDownKeyHit(args)) { return; }

            if (SpaceKeyHit(args)) { return; }
        }

        protected virtual bool UpOrDownKeyHit(KeyPressEventArgs args)
        {
            if (args == null)
            {
                return false;
            }

            if (args.KeyInfo.Key == ConsoleKey.UpArrow || args.KeyInfo.Key == ConsoleKey.DownArrow)
            {
                args.Cancel = true;

                if (args.KeyInfo.Key == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;
                }

                if (args.KeyInfo.Key == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                }

                return true;
            }

            return false;
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
                Checked = Options[SelectedIndex];

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

                SetCursorPosition(new Point(Position.X + 1, Position.Y + SelectedIndex));
            }
        }
    }
}
