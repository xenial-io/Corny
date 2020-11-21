using static Xenial.Delicious.Corny.Tests.CheckboxFacts;
using static Xenial.Delicious.Corny.Tests.RadioGroupFacts;
using static Xenial.Delicious.Corny.Tests.TextboxFacts;
using static Xenial.Tasty;

CheckboxTests();
TextboxTests();
RadioGroupTests();

await Run(args);
