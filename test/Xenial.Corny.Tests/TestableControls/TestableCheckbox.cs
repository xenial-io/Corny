using System.Drawing;

namespace Xenial.Delicious.Corny.Tests.TestableControls
{
    public class TestableCheckbox : Checkbox
    {
        public Point LastCursorLocation { get; private set; }

        protected override void SetCursorPosition(Point location)
            => LastCursorLocation = location;

        protected override void OnFocusedChanged() { }

        public override void Render() { }
    }
}
