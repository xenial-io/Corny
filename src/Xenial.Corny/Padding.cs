using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Xenial.Delicious.Corny
{
    [Serializable]
    public struct Padding : IEquatable<Padding>
    {
        private bool all;
        private int top;
        private int left;
        private int right;
        private int bottom;

        public static readonly Padding Empty = new Padding(0);

        public Padding(int all)
        {
            this.all = true;
            top = left = right = bottom = all;
        }

        public Padding(int left, int top, int right, int bottom)
        {
            this.top = top;
            this.left = left;
            this.right = right;
            this.bottom = bottom;
            all = this.top == this.left && this.top == this.right && this.top == this.bottom;
        }

        public int All
        {
            get => all ? top : -1;
            set
            {
                if (all != true || top != value)
                {
                    all = true;
                    top = left = right = bottom = value;
                }
            }
        }

        public int Bottom
        {
            get
            {
                if (all)
                {
                    return top;
                }
                return bottom;
            }
            set
            {
                if (all || bottom != value)
                {
                    all = false;
                    bottom = value;
                }
            }
        }

        public int Left
        {
            get
            {
                if (all)
                {
                    return top;
                }
                return left;
            }
            set
            {
                if (all || left != value)
                {
                    all = false;
                    left = value;
                }
            }
        }

        public int Right
        {
            get
            {
                if (all)
                {
                    return top;
                }
                return right;
            }
            set
            {
                if (all || right != value)
                {
                    all = false;
                    right = value;
                }
            }
        }

        public int Top
        {
            get => top;
            set
            {
                if (all || top != value)
                {
                    all = false;
                    top = value;
                }
            }
        }

        [Browsable(false)]
        public int Horizontal => Left + Right;

        [Browsable(false)]
        public int Vertical => Top + Bottom;

        [Browsable(false)]
        public Size Size => new Size(Horizontal, Vertical);

        public override bool Equals(object other)
        {
            if (other is Padding)
            {
                return (Padding)other == this;
            }

            return false;
        }
        public bool Equals(Padding other)
            => other == this;

        public static Padding operator +(Padding p1, Padding p2) => new Padding(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);

        public static Padding Add(Padding left, Padding right)
            => left + right;

        public static Padding operator -(Padding p1, Padding p2) => new Padding(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);

        public static Padding Subtract(Padding left, Padding right)
            => left + right;

        public static bool operator ==(Padding p1, Padding p2) => p1.Left == p2.Left && p1.Top == p2.Top && p1.Right == p2.Right && p1.Bottom == p2.Bottom;

        public static bool operator !=(Padding p1, Padding p2) => !(p1 == p2);

        public override int GetHashCode() =>
            Left
                ^ RotateLeft(Top, 8)
                ^ RotateLeft(Right, 16)
                ^ RotateLeft(Bottom, 24);

        private static int RotateLeft(int value, int nBits)
        {
            nBits = nBits % 32;
            return value << nBits | value >> 32 - nBits;
        }

        public override string ToString() => $"{{Left={Left.ToString(CultureInfo.CurrentCulture)},Top={Top.ToString(CultureInfo.CurrentCulture)},Right={Right.ToString(CultureInfo.CurrentCulture)},Bottom={Bottom.ToString(CultureInfo.CurrentCulture)}}}";


    }
}
