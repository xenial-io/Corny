using System;
using System.Drawing;
using System.Linq;

namespace Xenial.Delicious.Corny.Drawing
{
    public class TextPart : IRenderable
    {
        public Point Location { get; set; }

        public ConsoleColor BackColor { get; set; } = ConsoleColor.White;

        public ConsoleColor ForeColor { get; set; } = ConsoleColor.White;

        public string? Text { get; set; }

        public void Render()
        {
            Console.BackgroundColor = BackColor;
            Console.ForegroundColor = ForeColor;

            SetCursorPosition();

            Console.Write(Text);
        }

        protected virtual void SetCursorPosition()
            => SetCursorPosition(Location);

        protected virtual void SetCursorPosition(Point point)
            => Console.SetCursorPosition(point.X, point.Y);
    }
}
