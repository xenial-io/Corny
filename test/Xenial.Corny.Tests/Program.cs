using System.Threading.Tasks;

using static Xenial.Delicious.Corny.Tests.ControlFacts;
using static Xenial.Delicious.Corny.Tests.CheckboxFacts;
using static Xenial.Delicious.Corny.Tests.RadioGroupFacts;
using static Xenial.Delicious.Corny.Tests.TextboxFacts;
using static Xenial.Tasty;

namespace Xenial.Delicious.Corny.Tests
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            ControlTests();
            CheckboxTests();
            TextboxTests();
            RadioGroupTests();

            return await Run(args);
        }
    }
}

