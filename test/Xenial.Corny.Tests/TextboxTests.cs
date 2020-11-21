using System;

using Cornichon;

using Shouldly;

using Xenial.Delicious.Corny.Tests.TestableControls;

using static Xenial.Tasty;

namespace Xenial.Delicious.Corny.Tests
{
    public static class TextboxFacts
    {
        public static void TextboxTests() => Describe(nameof(Textbox), () =>
        {
            Textbox CreateTextbox()
                => new TestableTextbox { Width = 20 };

            void Keypress(Textbox textbox, ConsoleKey direction, char @char = ' ')
                => ((ISupportKeyPress)textbox).OnKeyPress(new KeyPressEventArgs(new ConsoleKeyInfo(@char, direction, false, false, false)));

            Describe("Cursor navigation", () =>
            {
                It("left arrow", () =>
                {
                    var textbox = CreateTextbox();
                    Scenario
                        .Given(() => textbox.CursorPosition.ShouldBe(0))
                        .And(() => Keypress(textbox, ConsoleKey.LeftArrow))
                        .Then(() => textbox.CursorPosition.ShouldBe(0));
                });

                It("right arrow", () =>
                {
                    var textbox = CreateTextbox();
                    Scenario
                        .Given(() => textbox.CursorPosition.ShouldBe(0))
                        .And(() => Keypress(textbox, ConsoleKey.RightArrow))
                        .Then(() => textbox.CursorPosition.ShouldBe(0));
                });

                It("complex", () =>
                {
                    var textbox = CreateTextbox();
                    Scenario
                        .Given(() => textbox.CursorPosition.ShouldBe(0))
                        .And(() => Keypress(textbox, ConsoleKey.RightArrow))
                        .Then(() => textbox.CursorPosition.ShouldBe(0))
                        .And(() => Keypress(textbox, ConsoleKey.A, 'a'))
                        .Then(() => textbox.CursorPosition.ShouldBe(1))
                        .And(() => Keypress(textbox, ConsoleKey.RightArrow))
                        .Then(() => textbox.CursorPosition.ShouldBe(1))
                        ;
                });

                It("Pos1", () =>
                {
                    var textbox = CreateTextbox();
                    Scenario
                        .Given(() => textbox.CursorPosition.ShouldBe(0))
                        .And(() => Keypress(textbox, ConsoleKey.A, 'a'))
                        .And(() => Keypress(textbox, ConsoleKey.B, 'b'))
                        .Then(() => textbox.CursorPosition.ShouldBe(2))
                        .And(() => Keypress(textbox, ConsoleKey.Home))
                        .Then(() => textbox.CursorPosition.ShouldBe(0))
                        ;
                });

                It("End", () =>
                {
                    var textbox = CreateTextbox();
                    Scenario
                        .Given(() => textbox.CursorPosition.ShouldBe(0))
                        .And(() => Keypress(textbox, ConsoleKey.A, 'a'))
                        .And(() => Keypress(textbox, ConsoleKey.B, 'b'))
                        .Then(() => textbox.CursorPosition = 0)
                        .And(() => Keypress(textbox, ConsoleKey.End))
                        .Then(() => textbox.CursorPosition.ShouldBe(2))
                        ;
                });

                It("Del", () =>
                {
                    var textbox = CreateTextbox();
                    Scenario
                        .Given(() => textbox.CursorPosition.ShouldBe(0))
                        .And(() => Keypress(textbox, ConsoleKey.A, 'a'))
                        .And(() => Keypress(textbox, ConsoleKey.B, 'b'))
                        .And(() => Keypress(textbox, ConsoleKey.Delete))
                        .Then(() => textbox.CursorPosition.ShouldBe(2))
                        .Then(() => textbox.Text.ShouldBe("ab"))
                        .And(() => Keypress(textbox, ConsoleKey.LeftArrow))
                        .And(() => Keypress(textbox, ConsoleKey.Delete))
                        .Then(() => textbox.CursorPosition.ShouldBe(1))
                        .Then(() => textbox.Text.ShouldBe("a"))
                        ;
                });

                Describe("Backspace", () =>
                {
                    It("simple", () =>
                    {
                        var textbox = CreateTextbox();
                        Scenario
                            .Given(() => textbox.CursorPosition.ShouldBe(0))
                            .And(() => Keypress(textbox, ConsoleKey.Backspace))
                            .Then(() => textbox.CursorPosition.ShouldBe(0))
                            ;
                    });

                    It("with text", () =>
                    {
                        var textbox = CreateTextbox();
                        Scenario
                            .Given(() => textbox.CursorPosition.ShouldBe(0))
                            .And(() => Keypress(textbox, ConsoleKey.A, 'a'))
                            .And(() => Keypress(textbox, ConsoleKey.B, 'b'))
                            .And(() => Keypress(textbox, ConsoleKey.C, 'c'))
                            .Then(() => textbox.Text.ShouldBe("abc"))
                            .And(() => Keypress(textbox, ConsoleKey.Backspace))
                            .Then(() => textbox.Text.ShouldBe("ab"))
                            ;
                    });

                    It("with text and movement", () =>
                    {
                        var textbox = CreateTextbox();
                        Scenario
                            .Given(() => textbox.CursorPosition.ShouldBe(0))
                            .And(() => Keypress(textbox, ConsoleKey.A, 'a'))
                            .And(() => Keypress(textbox, ConsoleKey.B, 'b'))
                            .And(() => Keypress(textbox, ConsoleKey.C, 'c'))
                            .Then(() => textbox.Text.ShouldBe("abc"))
                            .And(() => Keypress(textbox, ConsoleKey.LeftArrow))
                            .And(() => Keypress(textbox, ConsoleKey.Backspace))
                            .Then(() => textbox.Text.ShouldBe("ac"))
                            .Then(() => textbox.CursorPosition.ShouldBe(1))
                            ;
                    });
                });

            });

            It("inserts text with complex movement", () =>
            {
                var textbox = CreateTextbox();
                Scenario
                    .Given(() => textbox.CursorPosition.ShouldBe(0))
                    .And(() => Keypress(textbox, ConsoleKey.A, 'a'))
                    .And(() => Keypress(textbox, ConsoleKey.B, 'b'))
                    .And(() => Keypress(textbox, ConsoleKey.LeftArrow))
                    .Then(() => textbox.CursorPosition.ShouldBe(1))
                    .Then(() => textbox.Text.ShouldBe("ab"))
                    .And(() => Keypress(textbox, ConsoleKey.C, 'c'))
                    .Then(() => textbox.CursorPosition.ShouldBe(2))
                    .Then(() => textbox.Text.ShouldBe("acb"))
                    ;
            });
        });
    }
}
