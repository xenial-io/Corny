using System;

using Shouldly;

namespace Xenial.Delicious.Corny.Tests
{
    public class RadioGroupTests
    {
        [Scenario]
        public void UpArrowScenario(RadioGroup radioGroup)
        {
            "Given i have a RadioGroup with 3 Elements"
                .x(() => radioGroup = new TestableRadioGroup
                {
                    Options =
                    {
                        "Option1",
                        "Option2",
                        "Option3",
                    }
                });

            "The selected index should be 0"
                .x(() => radioGroup.SelectedIndex.ShouldBe(0));

            "When i press the up key"
                .x(() => ((ISupportKeyPress)radioGroup).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.UpArrow, false, false, false))));

            "The selected index should be still 0"
                .x(() => radioGroup.SelectedIndex.ShouldBe(0));
        }

        [Scenario]
        public void DownArrowScenario(RadioGroup radioGroup)
        {
            "Given i have a RadioGroup with 3 Elements"
                .x(() => radioGroup = new TestableRadioGroup
                {
                    Options =
                    {
                        "Option1",
                        "Option2",
                        "Option3",
                    }
                });

            "The selected index should be 0"
                .x(() => radioGroup.SelectedIndex.ShouldBe(0));

            "When i press the down key"
                .x(() => ((ISupportKeyPress)radioGroup).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.DownArrow, false, false, false))));

            "The selected index should be still 1"
                .x(() => radioGroup.SelectedIndex.ShouldBe(1));

            "When i press the down key again"
                .x(() => ((ISupportKeyPress)radioGroup).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.DownArrow, false, false, false))));

            "The selected index should be 2"
                .x(() => radioGroup.SelectedIndex.ShouldBe(2));

            "When i press the down key again"
                .x(() => ((ISupportKeyPress)radioGroup).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.DownArrow, false, false, false))));

            "The selected index should be still 2"
                .x(() => radioGroup.SelectedIndex.ShouldBe(2));
        }

        [Scenario]
        public void SelectTheSecondOptionScenario(RadioGroup radioGroup)
        {
            "Given i have a RadioGroup with 3 Elements"
                .x(() => radioGroup = new TestableRadioGroup
                {
                    Options =
                    {
                        "Option1",
                        "Option2",
                        "Option3",
                    }
                });

            "The selected index should be 0"
                .x(() => radioGroup.SelectedIndex.ShouldBe(0));

            "The Checked Options null"
                .x(() => radioGroup.Checked.ShouldBe(null));

            "When i press the down key"
                .x(() => ((ISupportKeyPress)radioGroup).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.DownArrow, false, false, false))));

            "The selected index should be 1"
                .x(() => radioGroup.SelectedIndex.ShouldBe(1));

            "The Checked Options is still null"
                .x(() => radioGroup.Checked.ShouldBe(null));

            "When i press the space key"
                .x(() => ((ISupportKeyPress)radioGroup).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false))));

            "The Checked Options is Option2"
                .x(() => radioGroup.Checked.ShouldBe("Option2"));
        }

    }
}
