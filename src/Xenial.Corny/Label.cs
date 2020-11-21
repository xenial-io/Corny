using System;
using System.Linq;

using Xenial.Delicious.Corny.Drawing;

namespace Xenial.Delicious.Corny
{
    public class Label : Control, IControl
    {
        public Label()
        {
            BackColor = ConsoleColor.White;
            ForeColor = ConsoleColor.White;
        }

        public string? Text { get; set; }

        public bool SupportsFocus => false;

        public override void Render()
            => new TextPart
            {
                Location = Position,
                Text = Text,
                BackColor = BackColor,
                ForeColor = ForeColor,
            }.Render();
    }
}
