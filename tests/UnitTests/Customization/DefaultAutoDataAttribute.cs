// © Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using System;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace Sitecore.MMS.Upload.UnitTests.Customization
{
    public class DefaultAutoDataAttribute : AutoDataAttribute
    {
        public DefaultAutoDataAttribute(Action<IFixture> fixtureConfigurator)
            : base(() =>
            {
                var fixture = new Fixture();
                fixture.Customize(new AutoNSubstituteCustomization
                {
                    ConfigureMembers = true
                });

                fixtureConfigurator?.Invoke(fixture);
                return fixture;
            })
        {
        }

        public DefaultAutoDataAttribute()
            : this(f => { })
        {
        }
    }

    public class DefaultInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public DefaultInlineAutoDataAttribute(params object[] values)
            : base(new DefaultAutoDataAttribute(), values)
        {
        }
    }

    public class AutoNSubPropertyDataAttribute : MemberAutoDataAttribute
    {
        public AutoNSubPropertyDataAttribute(string propertyName, params object[] values)
            : base(new DefaultAutoDataAttribute(), propertyName, values)
        {
        }
    }

    //Example how to use inline autodata
    //[Theory, DefaultInlineAutoData("the_file_name")]
    //public void Test2(string fileName, UploadLinkRequest requestModel)
    //{
    //    requestModel.Should().NotBeNull();
    //}
}
