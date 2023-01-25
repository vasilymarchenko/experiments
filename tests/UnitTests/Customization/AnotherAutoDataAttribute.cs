using System;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace UnitTests.Customization
{
    public class TestsExamples
    {
        [Theory, AnotherAutoData]
        public void Test1(Func<string> func)
        {
            int initialCounter = FuncSpecimenBuilder.InvocationCount;
            var s = func();
            func();
            func();

            // How to calculate invocation count? Where are 2 ways:
            // 1. Use mock objectsa from one of mock framework (Moq, NSubstitute, ...)
            // 2. Inject some counter. For example in SpecimenBuilder

            //it works only mocked object, created by NSubstitute
            //func.ReceivedCalls().Count().Should().Be(3);

            s.Should().Be("Hello World");
            (FuncSpecimenBuilder.InvocationCount - initialCounter).Should().Be(3);
        }
    }

    public class AnotherAutoDataAttribute: AutoDataAttribute
    {
        public AnotherAutoDataAttribute(Action<IFixture> fixtureConfigurator)
            : base(() =>
            {
                var fixture = new Fixture();
                fixture.Customize(new AutoNSubstituteCustomization
                {
                    ConfigureMembers = true
                });

                // One more way for customization. Looks more readale as for me
                fixture.Customizations.Add(new FuncSpecimenBuilder());

                fixtureConfigurator?.Invoke(fixture);
                return fixture;
            })
        {
        }

        public AnotherAutoDataAttribute()
            : this(f => { })
        {
        }
    }

    public class FuncSpecimenBuilder : ISpecimenBuilder
    {
        public static int InvocationCount { get; private set; }

        public object Create(object request, ISpecimenContext context)
        {
            if (request != typeof(Func<string>))
            {
                return new NoSpecimen();
            }

            var func = new Func<string>(() =>
            {
                InvocationCount++;
                return "Hello World";
            });
            return func;
        }
    }
}
