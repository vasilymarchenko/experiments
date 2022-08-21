using AutoFixture.Idioms;
using Sitecore.MMS.Upload.UnitTests.Customization;
using Xunit;

namespace UnitTests
{

    //Strange behaviour of GuardClauseAssertion -
    //If set Public constructors explicitly, it doesn't fail as expected
    //https://stackoverflow.com/questions/73433631/guardclauseassertion-does-not-fail-if-limited-to-only-public-constructors

    public class GuardAssertionExample
    {
        [Theory, DefaultAutoData]
        public void PublicMembers_Guarded(GuardClauseAssertion guard) =>
            guard.Verify(typeof(TestClass).GetConstructors(/*BindingFlags.Public*/));
    }

    public class TestBaseClass
    {
        private readonly string _firstDependency;
        private readonly string _secondDependency;

        protected TestBaseClass(string firstDependency, string secondDependency)
        {
            _firstDependency = firstDependency;
            _secondDependency = secondDependency;
        }
    }

    public class TestClass : TestBaseClass
    {
        public TestClass(string firstDependency)
          : base(firstDependency, string.Empty)
        {
        }
    }
}
