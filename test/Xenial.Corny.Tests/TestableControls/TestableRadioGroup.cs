using System.Drawing;

namespace Xenial.Delicious.Corny.Tests.TestableControls
{
    public class TestableRadioGroup : RadioGroup
    {
        public Point LastCursorLocation { get; private set; }

        protected override void SetCursorPosition(Point location)
            => LastCursorLocation = location;

        public override void Render() { }

        protected override void OnFocusedChanged() { }
    }
}
