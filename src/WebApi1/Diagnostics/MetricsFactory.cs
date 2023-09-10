using Prometheus;

namespace WebApi1.Diagnostics
{
    public static class MetricsFactory
    {
        public static Counter GetCounter(string name)
        {
            return GetCounter(name, Array.Empty<string>());
        }

        public static Counter GetCounter(string name, string label)
        {
            return GetCounter(name, new string[] { label });
        }

        public static Counter GetCounter(string name, string[] labels)
        {
            Counter counter = Metrics.CreateCounter(
                name,
                "some description should be here",
                new CounterConfiguration
                {
                    LabelNames = labels
                });

            return counter;
        }

        public static Histogram GetHistogram(string name, string? description = null)
        {
            return GetHistogram(name, Array.Empty<string>(), description);
        }

        public static Histogram GetHistogram(string name, string label, string? description = null)
        {
            return GetHistogram(name, new string[] { label }, description);
        }

        public static Histogram GetHistogram(string name, string[] labels, string? description = null)
        {
            description ??= "Prometheus histogram";

            var histogram = Metrics.CreateHistogram(
                name,
                description,
                new HistogramConfiguration
                {
                    LabelNames = labels,
                    Buckets = Histogram.LinearBuckets(start: 0.1, width: 0.1, count: 10)
                });

            return histogram;
        }
    }
}
