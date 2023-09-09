using Prometheus;

namespace WebApi1.Diagnostics
{
    public static class MetricsFactory
    {
        public static Counter GetCounter(string name)
        {
            Counter counter = Metrics.CreateCounter(
                name,
                "some description should be here"
                //new CounterConfiguration
                //{
                //    LabelNames = new[] { "parameter1", "parameter2" } // Replace with your parameter names
                //}
                );

            return counter;
        }
    }
}
