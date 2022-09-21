// © Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace Sitecore.MMS.Upload.UnitTests.Customization
{

    //Example how to inject different configuration based on parameter

    public class CustomConfigAutoDataAttribute : AutoDataAttribute
    {
        public CustomConfigAutoDataAttribute(Action<IFixture> fixtureConfigurator, string key)
            : base(() =>
            {
                var fixture = new Fixture();
                fixture.Customize(new AutoNSubstituteCustomization
                {
                    ConfigureMembers = true
                });

                //fixture.Freeze<AzureConfiguration>();

                //fixture.Customize<IConfiguration>(b => b.FromFactory(() =>
                //{
                //    return new ConfigurationBuilder()
                //        .AddInMemoryCollection(TestConfigurarionSettings.Configurations[key])
                //        .Build();
                //}));

                fixtureConfigurator?.Invoke(fixture);
                return fixture;
            })
        {
        }

        public CustomConfigAutoDataAttribute(string key)
            : this(f => { }, key)
        {
        }
    }

    public static class TestConfigurarionSettings
    {
        public static Dictionary<string, Dictionary<string, string>> Configurations =
            new Dictionary<string, Dictionary<string, string>>()
            {
                {"not-configured", new Dictionary<string, string>()
                    {
                        {"MdsSettings:SmthElse", "bla-bla-bla" }
                    }
                },
                {"well-configured", new Dictionary<string, string>()
                    {
                        {"MdsSettings:DeliveryUrl", "https://mds/delivery/{tenantId}/{key}" },
                        {"MdsSettings:UploadUrl", "https://upload.sitecore.io" }
                    }
                },
                {"no-tenant-placeholder", new Dictionary<string, string>()
                    {
                        {"MdsSettings:DeliveryUrl", "https://mds/delivery/wrong-tenant/{key}" }
                    }
                },
                {"no-key-placeholder", new Dictionary<string, string>()
                    {
                        {"MdsSettings:DeliveryUrl", "https://mds/delivery/{tenantId}/no-blob-key" }
                    }
                }
            };
    }
}
