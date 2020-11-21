using System;

using Cornichon;

using Shouldly;

using Xenial.Delicious.Corny.Tests.TestableControls;

using static Xenial.Tasty;

namespace Xenial.Delicious.Corny.Tests
{
    public static class RadioGroupFacts
    {
        public static void RadioGroupTests() => Describe(nameof(RadioGroup), () =>
        {
            RadioGroup CreateRadioGroup()
                => new TestableRadioGroup
                {
                    Options =
                    {
                        "Option1",
                        "Option2",
                        "Option3",
                    }
                };

            void Keypress(RadioGroup radioGroup, ConsoleKey direction, char @char = ' ')
               => ((ISupportKeyPress)radioGroup).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(@char, direction, false, false, false)));

            Describe("Keyboard navigtation", () =>
            {
                It("up arrow", () =>
                {
                    var radioGroup = CreateRadioGroup();
                    Scenario
                        .Given(() => radioGroup.SelectedIndex.ShouldBe(0))
                        .And(() => Keypress(radioGroup, ConsoleKey.UpArrow))
                        .Then(() => radioGroup.SelectedIndex.ShouldBe(0));
                });

                It("down arrow", () =>
                {
                    var radioGroup = CreateRadioGroup();
                    Scenario
                        .Given(() => radioGroup.SelectedIndex.ShouldBe(0))
                        .And(() => Keypress(radioGroup, ConsoleKey.DownArrow))
                        .Then(() => radioGroup.SelectedIndex.ShouldBe(1))
                        .And(() => Keypress(radioGroup, ConsoleKey.DownArrow))
                        .Then(() => radioGroup.SelectedIndex.ShouldBe(2))
                        .And(() => Keypress(radioGroup, ConsoleKey.DownArrow))
                        .Then(() => radioGroup.SelectedIndex.ShouldBe(2))
                    ;
                });
            });

            Describe("Selection", () =>
            {
                It("should work", () =>
                {
                    var radioGroup = CreateRadioGroup();
                    Scenario
                        .Given(() => radioGroup.SelectedIndex.ShouldBe(0))
                        .Then(() => radioGroup.Checked.ShouldBeNull())
                        .And(() => Keypress(radioGroup, ConsoleKey.DownArrow))
                        .Then(() => radioGroup.SelectedIndex.ShouldBe(1))
                        .Then(() => radioGroup.Checked.ShouldBeNull())
                        .And(() => Keypress(radioGroup, ConsoleKey.Spacebar))
                        .Then(() => radioGroup.Checked.ShouldBe("Option2"))
                    ;
                });
            });
        });
    }
}
