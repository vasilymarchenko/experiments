using AspectInjector.Broker;
using WebApi1.Diagnostics.Aspects;

namespace WebApi1.Diagnostics.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [Injection(typeof(TraceAspectMethodAround))]
    public class SimpleMetricsAttribute : Attribute
    {
        public string MetricName { get; init; }
        public string MetricDescr { get; init; }
        public string[] Labels = Array.Empty<string>();
        public int[] ArgOrder = Array.Empty<int>();

        public SimpleMetricsAttribute(): this(string.Empty, string.Empty)
        {
        }

        public SimpleMetricsAttribute(string metricName, string metricDescr):
            this(metricName, metricDescr, Array.Empty<string>(), Array.Empty<int>())
        {
        }

        public SimpleMetricsAttribute(
            string metricName,
            string metricDescr,
            string[] labels,
            int[] argOrder)
        {
            MetricName = metricName;
            MetricDescr = metricDescr;

            if (labels.Length != argOrder.Length)
            {
                throw new ArgumentException("Number of labels and args must be the same");
            }

            Labels = labels;
            ArgOrder = argOrder;
        }
    }
}
