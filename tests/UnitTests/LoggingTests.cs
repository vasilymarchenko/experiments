using FluentAssertions;
using MELT;
using Microsoft.Extensions.Logging;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class MyClass
    {
        private readonly ILogger<MyClass> _logger;
        public MyClass(ILogger<MyClass> logger)
        {
            _logger = logger;
        }

        public void DoWork()
        {
            _logger.LogInformation("DoWork is doing something useful");
        }
    }

    // Use MELT for testing of logs
    // https://github.com/alefranz/MELT
    public class LoggingTests
    {
        [Fact]
        public void Test1()
        {
            var loggerFactory = TestLoggerFactory.Create();
            var logger = loggerFactory.CreateLogger<MyClass>();

            var sut = new MyClass(logger);
            sut.DoWork();

            var logs = loggerFactory.Sink.LogEntries;
            logs.Count().Should().Be(1);
        }
    }
}
