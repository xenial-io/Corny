using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace Xenial.Delicious.Corny
{
    [DebuggerDisplay("Name: '{Name}'")]
    public abstract class Control : IControl
    {
        public ConsoleColor BackColor { get; set; } = ConsoleColor.White;

        public ConsoleColor ForeColor { get; set; } = ConsoleColor.DarkGray;

        public Point Location { get; set; }

        public Padding Margin { get; set; } = new Padding(1, 1, 1, 1);

        public Padding Padding { get; set; } = new Padding(0);

        public Point Offset => Parent == null ? Point.Empty : Parent.Position;

        public Point Position => new Point(Location.X + Offset.X + Margin.Left, Location.Y + Offset.Y + Margin.Top);

        public string? Name { get; set; }

        private IControl? parent;

        IList<IControl> IControl.Controls => throw new NotImplementedException();

        IControl? IControl.FocusedControl
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public IControl? Parent
        {
            get => parent;
            set
            {
                if (parent != null && parent.Controls.Contains(this))
                {
                    parent?.Controls.Remove(this);
                }

                parent = value;

                if (value != null)
                {
                    Location = new Point(Location.X + value.Padding.Left, Location.Y + value.Padding.Top);
                }

                if (parent != null && !parent.Controls.Contains(this))
                {
                    value?.Controls.Add(this);
                }
            }
        }

        private bool isFocused;
        bool IControl.IsFocused
        {
            get => isFocused;
            set
            {
                isFocused = value;
                OnFocusedChanged();
            }
        }

        protected virtual void OnFocusedChanged()
            => this.SetCursorVisible(true);

        bool IControl.SupportsFocus => false;

        public abstract void Render();

        protected virtual void SetCursorPosition(Point location)
        {
            this.SetCursorVisible(true);
            Console.SetCursorPosition(location.X, location.Y);
        }

        public virtual void Focus()
        {
            if (((IControl)this).SupportsFocus)
            {
                if (Parent != null)
                {
                    Parent.FocusedControl = this;
                }

                SetCursorPosition();
            }
        }

        protected virtual void SetCursorPosition() => SetCursorPosition(Position);

        void IControl.SelectNextControl() => throw new NotImplementedException();

        void IControl.SelectPreviousControl() => throw new NotImplementedException();

        void IRenderable.Render() => throw new NotImplementedException();
    }
}
