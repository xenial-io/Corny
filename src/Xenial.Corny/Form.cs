using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

using Xenial.Delicious.Corny.Drawing;

namespace Xenial.Delicious.Corny
{
    public class Form : Control,
        IControl,
        ISupportKeyPress,
        ISupportMouseClick
    {
        private List<IRenderable> Renderables { get; } = new List<IRenderable>();

        private readonly ObservableCollection<IControl> controls = new ControlCollection<IControl>();

        public event EventHandler<KeyPressEventArgs>? KeyPress;

        public event EventHandler<MouseClickEventArgs>? MouseClick;

        public IList<IControl> Controls => controls;

        public ConsoleColor BorderColor { get; set; } = ConsoleColor.Red;

        public ConsoleColor ShadowColor { get; set; } = ConsoleColor.DarkRed;

        public Size Size { get; set; }

        public string Title { get; set; }

        IControl? IControl.Parent
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        private IControl? focusedControl;
        public IControl? FocusedControl
        {
            get => focusedControl;
            set
            {
                foreach (var control in Controls)
                {
                    control.IsFocused = false;
                }

                if (focusedControl != value)
                {
                    focusedControl = value;

                    if (focusedControl != null)
                    {
                        focusedControl.IsFocused = true;
                    }

                    focusedControl?.Focus();
                }
            }
        }

        public bool SupportsFocus => true;

        public Form()
        {
            ForeColor = ConsoleColor.White;

            Margin = new Padding(0);
            Padding = new Padding(1, 2, 2, 1);
            Title = string.Empty;

            controls.CollectionChanged += Controls_CollectionChanged;
        }

        private void Controls_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            controls.CollectionChanged -= Controls_CollectionChanged;
            try
            {
                if (e.NewItems != null && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (var control in e.NewItems.OfType<IControl>())
                    {
                        control.Parent = this;
                    }
                }

                if (e.OldItems != null && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                {
                    foreach (var control in e.OldItems.OfType<IControl>())
                    {
                        control.Parent = null;
                    }
                }
            }
            finally
            {
                controls.CollectionChanged += Controls_CollectionChanged;
            }
        }

        public override void Render()
        {
            Renderables.Clear();

            var topShadow = new Line
            {
                Colour = ShadowColor,
                Length = Size.Width,
                Location = new Point(Position.X + 1, Position.Y),
                Orientation = Orientation.Horizontal,
            };

            var topBorder = new Line
            {
                Colour = BorderColor,
                Length = Size.Width,
                Location = new Point(Position.X, Position.Y + 1),
                Orientation = Orientation.Horizontal,
            };

            var leftBorder = new Line
            {
                Colour = BorderColor,
                Length = Size.Height - 1,
                Location = new Point(Position.X, Position.Y + 1),
                Orientation = Orientation.Vertical,
            };

            var rightBorder = new Line
            {
                Colour = BorderColor,
                Length = Size.Height - 1,
                Location = new Point(Position.X + Size.Width - 1, Position.Y + 1),
                Orientation = Orientation.Vertical,
            };

            var rightShadowBorder = new Line
            {
                Colour = ShadowColor,
                Length = Size.Height - 1,
                Location = new Point(Position.X + Size.Width, Position.Y + 1),
                Orientation = Orientation.Vertical,
            };

            var bottomBorder = new Line
            {
                Colour = BorderColor,
                Length = Size.Width,
                Location = new Point(Position.X, Position.Y + Size.Height),
                Orientation = Orientation.Horizontal,
            };

            Renderables.Add(topShadow);
            Renderables.Add(topBorder);

            Renderables.Add(new TextPart
            {
                Location = new Point(topBorder.Location.X + 2, topBorder.Location.Y),
                Text = Title,
                BackColor = BorderColor,
                ForeColor = ForeColor,
            });

            Renderables.Add(leftBorder);
            Renderables.Add(rightBorder);
            Renderables.Add(rightShadowBorder);
            Renderables.Add(bottomBorder);

            foreach (var renderable in Renderables)
            {
                renderable.Render();
            }

            foreach (var control in Controls)
            {
                control.Render();
            }
        }

        public override void Focus() => Controls?.FirstOrDefault(m => m.SupportsFocus)?.Focus();

        void ISupportKeyPress.OnKeyPress(KeyPressEventArgs args) => OnKeyPressCore(args);

        protected virtual void OnKeyPressCore(KeyPressEventArgs args)
        {
            if (args == null || args.Cancel)
            {
                return;
            }

            KeyPress?.Invoke(this, args);

            if (args.Cancel) { return; }

            if (FocusedControl is ISupportKeyPress keypressControl)
            {
                keypressControl.OnKeyPress(args);

                if (args.Cancel)
                {
                    return;
                }
            }

            foreach (var keyPress in Renderables.OfType<ISupportKeyPress>())
            {
                keyPress.OnKeyPress(args);

                if (args.Cancel)
                {
                    return;
                }
            }

            foreach (var keyPress in Controls.OfType<ISupportKeyPress>())
            {
                keyPress.OnKeyPress(args);

                if (args.Cancel)
                {
                    return;
                }
            }

            if (ShiftTabKeyHit(args))
            {
                return;
            }

            if (TabKeyHit(args))
            {
                return;
            }
        }

        protected virtual bool TabKeyHit(KeyPressEventArgs args)
        {
            if (args == null)
            {
                return false;
            }

            if (args.KeyInfo.Key == ConsoleKey.Tab)
            {
                SelectNextControl();
                return true;
            }

            return false;
        }

        protected virtual bool ShiftTabKeyHit(KeyPressEventArgs args)
        {
            if (args == null)
            {
                return false;
            }

            if (args.KeyInfo.Modifiers == ConsoleModifiers.Shift && args.KeyInfo.Key == ConsoleKey.Tab)
            {
                SelectPreviousControl();
                return true;
            }

            return false;
        }

        public void SelectNextControl()
        {
            if (FocusedControl == null)
            {
                FocusedControl = Controls.FirstOrDefault(m => m.SupportsFocus);
            }
            else
            {
                var focusedIndex = Controls.IndexOf(FocusedControl);

                for (var i = 0; i < Controls.Count; i++)
                {
                    if (i > focusedIndex)
                    {
                        var focusedControl = Controls[i];
                        if (focusedControl.SupportsFocus)
                        {
                            FocusedControl = focusedControl;
                            return;
                        }
                    }
                }

                FocusedControl = Controls.FirstOrDefault(m => m.SupportsFocus);
            }
        }

        public void SelectPreviousControl()
        {
            if (FocusedControl == null)
            {
                FocusedControl = Controls.LastOrDefault(m => m.SupportsFocus);
            }
            else
            {
                var focusedIndex = Controls.IndexOf(FocusedControl);

                for (var i = Controls.Count - 1; i > 0; i--)
                {
                    if (i < focusedIndex)
                    {
                        var focusedControl = Controls[i];
                        if (focusedControl.SupportsFocus)
                        {
                            FocusedControl = focusedControl;
                            return;
                        }
                    }
                }

                FocusedControl = Controls.LastOrDefault(m => m.SupportsFocus);
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

            foreach (var mouseClick in Controls.OfType<ISupportMouseClick>())
            {
                mouseClick.OnMouseClick(args);

                if (args.Cancel)
                {
                    return;
                }
            }
        }
    }
}
