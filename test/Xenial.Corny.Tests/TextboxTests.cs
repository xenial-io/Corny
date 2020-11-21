using System;

using Shouldly;

namespace Xenial.Delicious.Corny.Tests
{
    public class TextboxTests
    {
        [Scenario]
        public void LeftArrowScrenario(Textbox textbox)
        {
            "Given I have a Textbox"
                .x(() => textbox = new TestableTextbox
                {
                    Width = 20,
                });

            "The initial CursorPosition is 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));

            "When i press left"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.LeftArrow, false, false, false))));

            "The CursorPosition is still 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));
        }

        [Scenario]
        public void RightArrowScrenario(Textbox textbox)
        {
            "Given I have a Textbox"
                .x(() => textbox = new TestableTextbox
                {
                    Width = 20,
                });

            "The initial CursorPosition is 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));

            "When i press right"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.RightArrow, false, false, false))));

            "The CursorPosition is still 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));
        }

        [Scenario]
        public void ComplexArrowScrenarioWithText(Textbox textbox)
        {
            "Given I have a Textbox"
                .x(() => textbox = new TestableTextbox
                {
                    Width = 20,
                });

            "The initial CursorPosition is 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));

            "When i press right"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.RightArrow, false, false, false))));

            "The CursorPosition is still 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));

            "When i enter a"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false))));

            "The CursorPosition is 1"
                .x(() => textbox.CursorPosition.ShouldBe(1));

            "When i press right again"
               .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.RightArrow, false, false, false))));

            "The CursorPosition is still 1"
                .x(() => textbox.CursorPosition.ShouldBe(1));
        }

        [Scenario]
        public void Pos1Scenario(Textbox textbox)
        {
            "Given I have a Textbox"
                .x(() => textbox = new TestableTextbox
                {
                    Width = 20,
                });

            "The initial CursorPosition is 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));

            "When i enter a"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false))));

            "And then b"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('b', ConsoleKey.B, false, false, false))));

            "The CursorPosition is 2"
                .x(() => textbox.CursorPosition.ShouldBe(2));

            "When i Press Pos1"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.Home, false, false, false))));

            "The CursorPosition is 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));
        }

        [Scenario]
        public void EndScenario(Textbox textbox)
        {
            "Given I have a Textbox"
                .x(() => textbox = new TestableTextbox
                {
                    Width = 20,
                });

            "The initial CursorPosition is 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));

            "When i enter a"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false))));

            "And then b"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('b', ConsoleKey.B, false, false, false))));

            "When i set the CursorPosition to 0"
                .x(() => textbox.CursorPosition = 0);

            "When i Press End"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.End, false, false, false))));

            "The CursorPosition is 2"
                .x(() => textbox.CursorPosition.ShouldBe(2));
        }

        [Scenario]
        public void DelScenario(Textbox textbox)
        {
            "Given I have a Textbox"
                .x(() => textbox = new TestableTextbox
                {
                    Width = 20,
                });

            "The initial CursorPosition is 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));

            "When i enter a"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false))));

            "And then b"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('b', ConsoleKey.B, false, false, false))));

            "When i Press Del"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.Delete, false, false, false))));

            "The CursorPosition is 2"
                .x(() => textbox.CursorPosition.ShouldBe(2));

            "And the Text is still ab"
                .x(() => textbox.Text.ShouldBe("ab"));

            "But when i press left"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.LeftArrow, false, false, false))));

            "And then Del"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.Delete, false, false, false))));

            "The CursorPosition is 1"
                .x(() => textbox.CursorPosition.ShouldBe(1));

            "And the Text is a"
                .x(() => textbox.Text.ShouldBe("a"));
        }

        [Scenario]
        public void ComplexInsertScenario(Textbox textbox)
        {
            "Given I have a Textbox"
                .x(() => textbox = new TestableTextbox
                {
                    Width = 20,
                });

            "The initial CursorPosition is 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));

            "When i enter a"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false))));

            "And then b"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('b', ConsoleKey.B, false, false, false))));

            "But when i press left"
               .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.LeftArrow, false, false, false))));

            "The CursorPosition is 1"
                .x(() => textbox.CursorPosition.ShouldBe(1));

            "And the Text is still ab"
                .x(() => textbox.Text.ShouldBe("ab"));

            "When i now hit c"
                 .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('c', ConsoleKey.B, false, false, false))));

            "The CursorPosition is 2"
                .x(() => textbox.CursorPosition.ShouldBe(2));

            "And the Text is acb"
                .x(() => textbox.Text.ShouldBe("acb"));
        }

        [Scenario]
        public void BackspaceScrenario(Textbox textbox)
        {
            "Given I have a Textbox"
                .x(() => textbox = new TestableTextbox
                {
                    Width = 20,
                });

            "The initial CursorPosition is 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));

            "When i press left"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.Backspace, false, false, false))));

            "The CursorPosition is still 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));
        }

        [Scenario]
        public void BackspaceWithTextScenario(Textbox textbox)
        {
            "Given I have a Textbox"
                .x(() => textbox = new TestableTextbox
                {
                    Width = 20,
                });

            "The initial CursorPosition is 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));

            "When i enter a"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false))));

            "And then b"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('b', ConsoleKey.B, false, false, false))));

            "And then c"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('c', ConsoleKey.C, false, false, false))));

            "Text is abc"
               .x(() => textbox.Text.ShouldBe("abc"));

            "When I hit Backspace now"
               .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.Backspace, false, false, false))));

            "Text is ab"
               .x(() => textbox.Text.ShouldBe("ab"));
        }

        [Scenario]
        public void BackspaceWithTextAndMovementScenario(Textbox textbox)
        {
            "Given I have a Textbox"
                .x(() => textbox = new TestableTextbox
                {
                    Width = 20,
                });

            "The initial CursorPosition is 0"
                .x(() => textbox.CursorPosition.ShouldBe(0));

            "When i enter a"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false))));

            "And then b"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('b', ConsoleKey.B, false, false, false))));

            "And then c"
                .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo('c', ConsoleKey.C, false, false, false))));

            "Text is abc"
               .x(() => textbox.Text.ShouldBe("abc"));

            "When i press left"
               .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.LeftArrow, false, false, false))));

            "And then the Backspace key"
               .x(() => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(' ', ConsoleKey.Backspace, false, false, false))));

            "Text is ac"
               .x(() => textbox.Text.ShouldBe("ac"));

            "And the CursorPosition should be 1"
               .x(() => textbox.CursorPosition.ShouldBe(1));
        }
    }
}
