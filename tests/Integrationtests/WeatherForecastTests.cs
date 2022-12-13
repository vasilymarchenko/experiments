using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.IO;
using System.Net;
using Xunit;

namespace Integrationtests
{
    public class WeatherForecastTests: TestFixture
    {
        private readonly HttpClient _client;
        public WeatherForecastTests(WebApplicationFactory<Program> factory): base(factory)
        {
            _client = ConfigureClient();
        }

        [Fact]
        public async Task Test1()
        {
            HttpResponseMessage? response = await _client.GetAsync("WeatherForecast").ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
