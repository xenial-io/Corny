using Shouldly;

namespace Xenial.Delicious.Corny.Tests
{
    public class ControlTests
    {
        private readonly Form Form;

        private readonly Textbox Textbox;
        private readonly Label Label;
        private readonly Textbox Textbox2;
        private readonly Label Label2;
        private readonly Label Label3;
        private readonly Checkbox Checkbox;

        public ControlTests()
        {
            Form = new Form();
            Textbox = new TestableTextbox();
            Label = new TestableLabel();
            Textbox2 = new TestableTextbox();
            Label2 = new TestableLabel();
            Checkbox = new TestableCheckbox();
            Label3 = new TestableLabel();
        }

        public class ParentTests : ControlTests
        {
            [Fact]
            public void SettingTheParentPopulatesChildCollection()
            {
                Textbox.Parent = Form;

                Form.Controls.ShouldContain(Textbox);
            }

            [Fact]
            public void SettingTheParentAndRemovingParentRemovesFromTheParentsCollection()
            {
                Textbox.Parent = Form;

                Textbox.Parent = null;

                Form.Controls.ShouldNotContain(Textbox);
            }

            [Fact]
            public void SettingTheParentTwiceOnlyAddsOne()
            {
                Textbox.Parent = Form;

                Textbox.Parent = Form;

                Form.Controls.Count.ShouldBe(1);
            }
        }


        public class ChildrenTests : ControlTests
        {
            [Fact]
            public void AddingToTheChildrenCollectionSetsParent()
            {
                Form.Controls.Add(Textbox);

                Textbox.Parent.ShouldBe(Form);
            }

            [Fact]
            public void AddingToTheChildrenCollectionTwiceSetsParent()
            {
                Form.Controls.Add(Textbox);
                Form.Controls.Add(Textbox);

                Textbox.ShouldSatisfyAllConditions(
                    () => Textbox.Parent.ShouldBe(Form),
                    () => Form.Controls.Count.ShouldBe(1)
                );
            }

            [Fact]
            public void RemovingFromTheChildrenCollectionUnsetsParent()
            {
                Form.Controls.Add(Textbox);
                Form.Controls.Remove(Textbox);

                Textbox.ShouldSatisfyAllConditions(
                    () => Textbox.Parent.ShouldBeNull(),
                    () => Form.Controls.Count.ShouldBe(0)
                );
            }

            [Fact]
            public void ClearFromTheChildrenCollectionUnsetsParent()
            {
                Form.Controls.Add(Textbox);
                Form.Controls.Clear();

                Textbox.ShouldSatisfyAllConditions(
                    () => Textbox.Parent.ShouldBeNull(),
                    () => Form.Controls.Count.ShouldBe(0)
                );
            }
        }

        public class FocusTests : ControlTests
        {
            [Fact]
            public void FocusWithMultipleElementsFocusesTheFirstFocusableControl()
            {
                Form.Controls.Add(Label);
                Form.Controls.Add(Textbox);

                Form.FocusedControl = Textbox;
            }
        }

        public class SelectNextControlTests : ControlTests
        {
            [Scenario]
            public void SelectNextControlDoesSimple()
            {
                "Given i got a normal form with 2 Labels and 2 Textboxes"
                    .x(() =>
                    {
                        Form.Controls.Add(Label);
                        Form.Controls.Add(Label2);
                        Form.Controls.Add(Textbox);
                        Form.Controls.Add(Textbox2);
                    });

                "And i call Form.SelectNextControl once"
                    .x(() => Form.SelectNextControl());

                "Textbox should be the focused Control"
                    .x(() => Form.FocusedControl.ShouldBe(Textbox));

                "When i call Form.SelectNextControl again"
                    .x(() => Form.SelectNextControl());

                "Textbox2 should be the focused Control"
                    .x(() => Form.FocusedControl.ShouldBe(Textbox2));

                "When i call Form.SelectNextControl again"
                  .x(() => Form.SelectNextControl());

                "Textbox should be the focused Control"
                    .x(() => Form.FocusedControl.ShouldBe(Textbox));
            }

            [Scenario]
            public void SelectNextControlDoesComplex()
            {
                "Given i got a normal form with 3 Labels, 2 Textboxes and one Checkbox"
                    .x(() =>
                    {
                        Form.Controls.Add(Label);
                        Form.Controls.Add(Label2);
                        Form.Controls.Add(Textbox);
                        Form.Controls.Add(Textbox2);
                        Form.Controls.Add(Label3);
                        Form.Controls.Add(Checkbox);
                    });

                "And i call Form.SelectNextControl once"
                    .x(() => Form.SelectNextControl());

                "Textbox should be the focused Control"
                    .x(() => Form.FocusedControl.ShouldBe(Textbox));

                "When i call Form.SelectNextControl again"
                    .x(() => Form.SelectNextControl());

                "Textbox2 should be the focused Control"
                    .x(() => Form.FocusedControl.ShouldBe(Textbox2));

                "When i call Form.SelectNextControl again"
                  .x(() => Form.SelectNextControl());

                "Checkbox should be the focused Control"
                    .x(() => Form.FocusedControl.ShouldBe(Checkbox));

                "When i call Form.SelectNextControl again"
                  .x(() => Form.SelectNextControl());

                "Textbox should be the focused Control"
                   .x(() => Form.FocusedControl.ShouldBe(Textbox));
            }
        }

        public class SelectPreviousControlTests : ControlTests
        {
            [Scenario]
            public void SelectPreviousControlDoesSimple()
            {
                "Given i got a normal form with 2 Labels and 2 Textboxes"
                    .x(() =>
                    {
                        Form.Controls.Add(Label);
                        Form.Controls.Add(Label2);
                        Form.Controls.Add(Textbox);
                        Form.Controls.Add(Textbox2);
                    });

                "And i call Form.SelectPreviousControl once"
                    .x(() => Form.SelectPreviousControl());

                "Textbox2 should be the focused Control"
                    .x(() => Form.FocusedControl.ShouldBe(Textbox2));

                "When i call Form.SelectPreviousControl again"
                    .x(() => Form.SelectPreviousControl());

                "Textbox should be the focused Control"
                    .x(() => Form.FocusedControl.ShouldBe(Textbox));

                "When i call Form.SelectPreviousControl again"
                  .x(() => Form.SelectPreviousControl());

                "Textbox2 should be the focused Control"
                    .x(() => Form.FocusedControl.ShouldBe(Textbox2));
            }
        }

        [Scenario]
        public void SelectPreviousControlDoesComplex()
        {
            "Given i got a normal form with 3 Labels, 2 Textboxes and one Checkbox"
                .x(() =>
                {
                    Form.Controls.Add(Label);
                    Form.Controls.Add(Label2);
                    Form.Controls.Add(Label3);
                    Form.Controls.Add(Textbox);
                    Form.Controls.Add(Textbox2);
                    Form.Controls.Add(Checkbox);
                });

            "And i call Form.SelectPreviousControl once"
                .x(() => Form.SelectPreviousControl());

            "Checkbox should be the focused Control"
                .x(() => Form.FocusedControl.ShouldBe(Checkbox));

            "When i call Form.SelectPreviousControl again"
                .x(() => Form.SelectPreviousControl());

            "Textbox2 should be the focused Control"
                .x(() => Form.FocusedControl.ShouldBe(Textbox2));

            "When i call Form.SelectPreviousControl again"
              .x(() => Form.SelectPreviousControl());

            "Textbox should be the focused Control"
                .x(() => Form.FocusedControl.ShouldBe(Textbox));

            "When i call Form.SelectPreviousControl again"
              .x(() => Form.SelectPreviousControl());

            "Checkbox should be the focused Control"
               .x(() => Form.FocusedControl.ShouldBe(Checkbox));
        }
    }
}
