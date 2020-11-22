using Cornichon;

using Shouldly;

using Xenial.Delicious.Corny.Tests.TestableControls;

using static Xenial.Tasty;


namespace Xenial.Delicious.Corny.Tests
{
    public static class ControlFacts
    {
        public static void ControlTests() => Describe("Controls", () =>
        {
            Form CreateForm() => new Form();
            Textbox CreateTextbox() => new TestableTextbox();
            Label CreateLabel() => new Label();
            Checkbox CreateCheckbox() => new TestableCheckbox();

            Describe(nameof(Control.Parent), () =>
            {
                It($"setting {nameof(Control.Parent)} populates {nameof(Form.Controls)}", () =>
                {
                    var form = CreateForm();
                    var textbox = CreateTextbox();
                    textbox.Parent = form;
                    form.Controls.ShouldContain(textbox);
                });

                It($"unsetting {nameof(Control.Parent)} removes from {nameof(Form.Controls)}", () =>
                {
                    var form = CreateForm();
                    var textbox = CreateTextbox();

                    Scenario
                        .Given(() => textbox.Parent = form)
                        .Then(() => form.Controls.ShouldContain(textbox))
                        .And(() => textbox.Parent = null)
                        .Then(() => form.Controls.ShouldNotContain(textbox))
                        .Then(() => textbox.Parent.ShouldBeNull());
                });

                It($"setting {nameof(Control.Parent)} twice adds only one to {nameof(Form.Controls)}", () =>
                {
                    var form = CreateForm();
                    var textbox = CreateTextbox();

                    Scenario
                        .Given(() => textbox.Parent = form)
                        .And(() => textbox.Parent = form)
                        .Then(() => form.Controls.ShouldContain(textbox))
                        .Then(() => form.Controls.Count.ShouldBe(1));
                });
            });

            Describe(nameof(Form.Controls), () =>
            {
                It($"adding to {nameof(Form.Controls)} sets {nameof(Control.Parent)}", () =>
                {
                    var form = CreateForm();
                    var textbox = CreateTextbox();
                    form.Controls.Add(textbox);
                    textbox.Parent.ShouldBe(form);
                });

                It($"adding to {nameof(Form.Controls)} twice sets {nameof(Control.Parent)}", () =>
                {
                    var form = CreateForm();
                    var textbox = CreateTextbox();
                    form.Controls.Add(textbox);
                    form.Controls.Add(textbox);

                    form.ShouldSatisfyAllConditions(
                        () => textbox.Parent.ShouldBe(form),
                        () => form.Controls.ShouldContain(textbox),
                        () => form.Controls.Count.ShouldBe(1)
                    );
                });

                It($"removing from {nameof(Form.Controls)} unsets {nameof(Control.Parent)}", () =>
                {
                    var form = CreateForm();
                    var textbox = CreateTextbox();
                    form.Controls.Add(textbox);
                    form.Controls.Remove(textbox);

                    form.ShouldSatisfyAllConditions(
                        () => textbox.Parent.ShouldBeNull(),
                        () => form.Controls.ShouldNotContain(textbox),
                        () => form.Controls.Count.ShouldBe(0)
                    );
                });

                It($"clear {nameof(Form.Controls)} unsets {nameof(Control.Parent)}", () =>
                {
                    var form = CreateForm();
                    var textbox = CreateTextbox();
                    form.Controls.Add(textbox);
                    form.Controls.Clear();

                    form.ShouldSatisfyAllConditions(
                        () => textbox.Parent.ShouldBeNull(),
                        () => form.Controls.ShouldNotContain(textbox),
                        () => form.Controls.Count.ShouldBe(0)
                    );
                });
            });

            Describe("Focus", () =>
            {
                It("should focus first focusable control", () =>
                {
                    var form = CreateForm();
                    var textbox = CreateTextbox();
                    var label = CreateLabel();
                    form.Controls.Add(label);
                    form.Controls.Add(textbox);

                    form.FocusedControl = textbox;
                    form.FocusedControl.ShouldBe(textbox);
                });
            });

            Describe(nameof(Form.SelectNextControl), () =>
            {
                It("does simple rotation", () =>
                {
                    var (form, label1, label2, textbox1, textbox2)
                        = (CreateForm(), CreateLabel(), CreateLabel(), CreateTextbox(), CreateTextbox());

                    form.Controls.Add(label1);
                    form.Controls.Add(label2);
                    form.Controls.Add(textbox1);
                    form.Controls.Add(textbox2);

                    Scenario
                        .Given(() => form.SelectNextControl())
                        .Then(() => form.FocusedControl.ShouldBe(textbox1))
                        .And(() => form.SelectNextControl())
                        .Then(() => form.FocusedControl.ShouldBe(textbox2))
                        .And(() => form.SelectNextControl())
                        .Then(() => form.FocusedControl.ShouldBe(textbox1));
                });

                It("does complex rotation", () =>
                {
                    var (form, label1, label2, textbox1, textbox2, label3, checkbox1)
                        = (CreateForm(), CreateLabel(), CreateLabel(), CreateTextbox(), CreateTextbox(), CreateLabel(), CreateCheckbox());

                    form.Controls.Add(label1);
                    form.Controls.Add(label2);
                    form.Controls.Add(textbox1);
                    form.Controls.Add(textbox2);
                    form.Controls.Add(label3);
                    form.Controls.Add(checkbox1);

                    Scenario
                        .Given(() => form.SelectNextControl())
                        .Then(() => form.FocusedControl.ShouldBe(textbox1))
                        .And(() => form.SelectNextControl())
                        .Then(() => form.FocusedControl.ShouldBe(textbox2))
                        .And(() => form.SelectNextControl())
                        .Then(() => form.FocusedControl.ShouldBe(checkbox1))
                        .And(() => form.SelectNextControl())
                        .Then(() => form.FocusedControl.ShouldBe(textbox1));
                });
            });

            Describe(nameof(Form.SelectPreviousControl), () =>
            {
                It("does simple rotation", () =>
                {
                    var (form, label1, label2, textbox1, textbox2)
                        = (CreateForm(), CreateLabel(), CreateLabel(), CreateTextbox(), CreateTextbox());

                    form.Controls.Add(label1);
                    form.Controls.Add(label2);
                    form.Controls.Add(textbox1);
                    form.Controls.Add(textbox2);

                    Scenario
                        .Given(() => form.SelectPreviousControl())
                        .Then(() => form.FocusedControl.ShouldBe(textbox2))
                        .And(() => form.SelectPreviousControl())
                        .Then(() => form.FocusedControl.ShouldBe(textbox1))
                        .And(() => form.SelectPreviousControl())
                        .Then(() => form.FocusedControl.ShouldBe(textbox2));
                });

                It("does complex rotation", () =>
                {
                    var (form, label1, label2, textbox1, textbox2, label3, checkbox1)
                        = (CreateForm(), CreateLabel(), CreateLabel(), CreateTextbox(), CreateTextbox(), CreateLabel(), CreateCheckbox());

                    form.Controls.Add(label1);
                    form.Controls.Add(label2);
                    form.Controls.Add(label3);
                    form.Controls.Add(textbox1);
                    form.Controls.Add(textbox2);
                    form.Controls.Add(checkbox1);

                    Scenario
                        .Given(() => form.SelectPreviousControl())
                        .Then(() => form.FocusedControl.ShouldBe(checkbox1))
                        .And(() => form.SelectPreviousControl())
                        .Then(() => form.FocusedControl.ShouldBe(textbox2))
                        .And(() => form.SelectPreviousControl())
                        .Then(() => form.FocusedControl.ShouldBe(textbox1))
                        .And(() => form.SelectPreviousControl())
                        .Then(() => form.FocusedControl.ShouldBe(checkbox1));
                });
            });
        });
    }
}
