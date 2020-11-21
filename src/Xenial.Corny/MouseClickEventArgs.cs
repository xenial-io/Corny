using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace Xenial.Delicious.Corny
{
    [DebuggerDisplay("X: {Location.X} Y: {Location.Y} Button: {Button} Console: {KeyInfo}")]
    public class MouseClickEventArgs : CancelEventArgs
    {
        public ConsoleKeyInfo KeyInfo { get; }

        public MouseButton Button { get; }

        public Point Location { get; }

        public MouseClickEventArgs(Point location, MouseButton button, ConsoleKeyInfo keyInfo)
        {
            KeyInfo = keyInfo;
            Location = location;
            Button = button;
        }
    }
}
