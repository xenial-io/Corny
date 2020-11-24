using System;

using Xenial.Delicious.Corny.Utils;

namespace Xenial.Delicious.Corny.Data
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Is record type")]
    public record Color(byte Red, byte Green, byte Blue)
    {
        public static implicit operator Color(ConsoleColor color) => ColorConverter.GetColor(color);

        public static Color ToColor(ConsoleColor color) => (Color)color;

        public Color Mix(in Color color, float factor) => this * (1 - factor) + color * factor;

        public static Color White => new(255, 255, 255);
        public static Color Black => new(0, 0, 0);

        public static Color operator *(in Color color, float factor)
        {
            _ = color ?? throw new ArgumentNullException(nameof(color));
            return new Color((byte)(color.Red * factor), (byte)(color.Green * factor), (byte)(color.Blue * factor));
        }

        public static Color Multiply(in Color lhs, in Color rhs) => lhs + rhs;

        public static Color operator +(in Color lhs, in Color rhs)
        {
            _ = lhs ?? throw new ArgumentNullException(nameof(lhs));
            _ = rhs ?? throw new ArgumentNullException(nameof(rhs));

            return new Color(
                (byte)Math.Min(byte.MaxValue, lhs.Red + rhs.Red),
                (byte)Math.Min(byte.MaxValue, lhs.Green + rhs.Green),
                (byte)Math.Min(byte.MaxValue, lhs.Blue + rhs.Blue)
            );
        }
        public static Color Add(in Color lhs, in Color rhs) => lhs + rhs;
    }
}
