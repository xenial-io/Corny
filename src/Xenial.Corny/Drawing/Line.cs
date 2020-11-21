using System;
using System.Drawing;
using System.Linq;

namespace Xenial.Delicious.Corny.Drawing
{
    public class Line : IRenderable
    {
        public Point Location { get; set; }

        public int Length { get; set; }

        public ConsoleColor Colour { get; set; } = ConsoleColor.White;

        public Orientation Orientation { get; set; } = Orientation.Horizontal;

        public void Render()
        {
            Console.BackgroundColor = Colour;

            if (Orientation == Orientation.Horizontal)
            {
                Console.SetCursorPosition(Location.X, Location.Y);
                Console.Write(new string(' ', Length));
            }
            else
            {
                var x = Location.X;

                for (var i = Location.Y; i < Location.Y + Length; i++)
                {
                    Console.SetCursorPosition(x, i);
                    Console.Write(" ");
                }
            }
        }
    }
}
