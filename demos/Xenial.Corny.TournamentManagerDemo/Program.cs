using System;

using Xenial.Delicious.Corny;

namespace Xenial.Corny.TournamentManagerDemo
{
    internal static class Program
    {
        internal static void Main(string[] _)
        {
            var form = new Form
            {
                Title = "Tournament Manager",
                Location = new System.Drawing.Point(4, 2),
                Size = new System.Drawing.Size(40, 20),
                Controls =
                {
                    new Button
                    {
                        Name = "playersButton",
                        Location = new System.Drawing.Point(0, 0),
                        Text = "Players",
                        Width = 36
                    },
                    new Button
                    {
                        Name = "exitButton",
                        Location = new System.Drawing.Point(0, 14),
                        Text = "Exit",
                        Width = 36
                    },
                }
            };

            var screen = new Screen
            {
                Renderables =
                    {
                        form,
                    }
            };

            screen.SetSize(50, 120);

            form.Focus();

            screen.Run();
        }
    }
}
