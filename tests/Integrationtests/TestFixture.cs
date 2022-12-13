using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Integrationtests
{
    public class TestFixture : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public TestFixture(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        public HttpClient ConfigureClient(bool allowRedirect = true)
        {
            return _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = allowRedirect
            });
        }
    }
}
