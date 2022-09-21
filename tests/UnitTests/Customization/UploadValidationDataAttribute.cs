// © Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Options;
using Sitecore.MMS.Upload.Models;
using System;
using System.Collections.Generic;

namespace Sitecore.MMS.Upload.UnitTests.Customization
{
    public class UploadValidationInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        //public UploadValidationInlineAutoDataAttribute(string fileName, params object[] values)
        //    : base(new UploadValidationDataAttribute(fileName), values)
        //{
        //}
        public UploadValidationInlineAutoDataAttribute(params object[] values)
            : base(new UploadValidationDataAttribute(values), values)
        {
        }
    }

    public class UploadValidationDataAttribute: AutoDataAttribute
    {
        //public UploadValidationDataAttribute(string fileName)
        //    : this(f => { }, fileName)
        //{
        //}

        public UploadValidationDataAttribute(object[] values)
            : this(f => { }, (string)values[0])
        {
        }

        public UploadValidationDataAttribute(Action<IFixture> fixtureConfigurator, string fileName)
            : base(() =>
            {
                var fixture = new Fixture();
                fixture.Customize(new AutoNSubstituteCustomization
                {
                    ConfigureMembers = true
                });

                fixture.Customize<UploadLinkRequest>(f => f.FromFactory(() => 
                    new UploadLinkRequest() { FileName = fileName })
                    .Without(m => m.FileName));

                var settings = new ValidationSettings()
                {
                    AllowedFileTypes = new List<string>() { "jpg", "gif", "png"}
                };
                fixture.Customize<IOptions<ValidationSettings>>(f => f.FromFactory(() => 
                    Options.Create(settings)));

                fixtureConfigurator?.Invoke(fixture);
                return fixture;
            })
        { 
        }
    }
}
