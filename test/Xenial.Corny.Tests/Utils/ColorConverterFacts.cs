using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shouldly;

using Xenial.Delicious.Corny.Utils;

using static Xenial.Tasty;

namespace Xenial.Delicious.Corny.Tests.Utils
{
    public static class ColorConverterFacts
    {
        public static void ColorConverterTests() => Describe(nameof(ColorConverter), () =>
        {
            var colors = new[]
            {
                ConsoleColor.Black,
                ConsoleColor.DarkBlue,
                ConsoleColor.DarkGreen,
                ConsoleColor.DarkCyan,
                ConsoleColor.DarkRed,
                ConsoleColor.DarkMagenta,
                ConsoleColor.DarkYellow,
                ConsoleColor.Gray,
                ConsoleColor.DarkGray,
                ConsoleColor.Blue,
                ConsoleColor.Green,
                ConsoleColor.Cyan,
                ConsoleColor.Red,
                ConsoleColor.Magenta,
                ConsoleColor.Yellow,
                ConsoleColor.White
            };

            foreach (var initialColor in colors)
            {
                It($"ConsoleColor {initialColor} is restored correctly", () =>
                {
                    var color = ColorConverter.GetColor(initialColor);
                    var convertedColor = ColorConverter.GetNearestConsoleColor(color);

                    initialColor.ShouldBe(convertedColor);
                });
            }
        });
    }
}
