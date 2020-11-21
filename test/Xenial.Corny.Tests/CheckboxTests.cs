using System;

using Cornichon;

using FakeItEasy;

using Shouldly;

using Xenial.Delicious.Corny.Tests.TestableControls;

using static Xenial.Tasty;

namespace Xenial.Delicious.Corny.Tests
{
    public static class CheckboxFacts
    {
        public static void CheckboxTests() => Describe(nameof(Checkbox), () =>
        {
            TestableCheckbox CreateCheckbox() => new();
            static void PressSpaceBar(TestableCheckbox checkbox) => ((ISupportKeyPress)checkbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false)));

            It("reacts to keyboard with spacebar", () =>
            {
                var checkbox = CreateCheckbox();

                Scenario
                    .Given(() => PressSpaceBar(checkbox))
                    .Then(() => checkbox.Checked.ShouldBeTrue())
                    .And(() => PressSpaceBar(checkbox))
                    .Then(() => checkbox.Checked.ShouldBeFalse());
            });

            It($"{nameof(Checkbox.CheckedChanged)} gets triggered", () =>
            {
                var checkbox = CreateCheckbox();
                var handler = A.Fake<EventHandler>();
                Scenario
                    .Given(() => checkbox.CheckedChanged += handler)
                    .And(() => PressSpaceBar(checkbox))
                    .And(() => PressSpaceBar(checkbox))
                    .Then(() => A.CallTo(() => handler.Invoke(A<object>.Ignored, A<EventArgs>.Ignored)).MustHaveHappenedTwiceExactly());
            });
        });
    }
}
