using System.Drawing;

namespace Xenial.Delicious.Corny.Tests.TestableControls
{
    public class TestableLabel : Label
    {
        public Point LastCursorLocation { get; private set; }

        protected override void SetCursorPosition(Point location)
            => LastCursorLocation = location;

        protected override void OnFocusedChanged() { }
    }
}
