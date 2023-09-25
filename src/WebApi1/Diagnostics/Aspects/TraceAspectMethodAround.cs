using System.Diagnostics;
using AspectInjector.Broker;
using WebApi1.Diagnostics.Attributes;
using WebApi1.Services;
using Target = AspectInjector.Broker.Target;

namespace WebApi1.Diagnostics.Aspects
{
    [Aspect(Scope.Global, Factory = typeof(AspectsFactory))]
    public class TraceAspectMethodAround
    {
        private readonly ILogger<TestWorker> _logger;

        public TraceAspectMethodAround(ILogger<TestWorker> logger)
        {
            _logger = logger;
        }

        [Advice(Kind.Around, Targets = Target.Method)]
        public object? HandleMethod(
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.Target)] Func<object[], object> method,
            [Argument(Source.ReturnType)] Type returnType,
            [Argument(Source.Triggers)] Attribute[] triggers,
            [Argument(Source.Name)] string methodName,
            [Argument(Source.Type)] Type targetType)
        {
            _ = method ?? throw new ArgumentNullException(nameof(method));
            _ = returnType ?? throw new ArgumentNullException(nameof(returnType));
            _ = targetType ?? throw new ArgumentNullException(nameof(targetType));

            var attributes = triggers.Where(a =>
                a is SimpleMetricsAttribute).ToArray();
            if (attributes.Length == 0)
            {
                throw new ArgumentException("No " + nameof(SimpleMetricsAttribute) + " found for the aspect.");
            }

            if (attributes.Length > 1)
            {
                throw new ArgumentException("Only a single attribute either " + nameof(SimpleMetricsAttribute) + " supported.");
            }

            var parameters = new Parameters
            {
                Attribute = attributes[0],
                Args = arguments,
                Target = method,
                MethodName = methodName,
                TypeName = targetType.Name
            };

            if (typeof(Task) == returnType)
            {
                return WrapVoidAsync(parameters);
            }

            if (typeof(void) == returnType)
            {
                WrapVoid(parameters);
                return null;
            }

            //if (!typeof(Task).IsAssignableFrom(returnType))
            //{
            //    return Wrap(parameters);
            //}


            return null;
        }

        private static void WrapVoid(Parameters parameters)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                parameters.Target(parameters.Args);
            }
            finally
            {
                stopwatch.Stop();
                Console.Write($"Elapsed {stopwatch.Elapsed.TotalSeconds} seconds");

                var attr = parameters.Attribute as SimpleMetricsAttribute;
                // TODO: we need validate attr


                var metric = MetricsFactory.GetHistogram(
                    GetMetricName(parameters),
                    GetLabelsNames(parameters),
                    GetMetricDescr(parameters));

                var labels = GetLabelsValues(parameters);
                if (labels.Length > 0)
                {
                    metric.WithLabels(labels);
                }
                metric.Observe(stopwatch.Elapsed.TotalSeconds);
            }
        }

        private static string GetMetricName(Parameters parameters)
        {
            var attr = parameters.Attribute as SimpleMetricsAttribute;
            if (attr != null && !string.IsNullOrWhiteSpace(attr.MetricName))
            {
                return attr.MetricName;
            }

            return $"{parameters.TypeName}_{parameters.MethodName}_duration";
        }

        private static string GetMetricDescr(Parameters parameters)
        {
            var attr = parameters.Attribute as SimpleMetricsAttribute;
            if (attr != null && !string.IsNullOrWhiteSpace(attr.MetricDescr))
            {
                return attr.MetricDescr;
            }

            return $"Duration of {parameters.TypeName}.{parameters.MethodName}";
        }

        private static string[] GetLabelsNames(Parameters parameters)
        {
            var attr = parameters.Attribute as SimpleMetricsAttribute;
            if (attr != null && attr.Labels.Length > 0)
            {
                return attr.Labels;
            }

            return Array.Empty<string>();
        }

        private static string[] GetLabelsValues(Parameters parameters)
        {
            var attr = parameters.Attribute as SimpleMetricsAttribute;
            if (attr != null && attr.ArgOrder.Length > 0)
            {
                var args = new string[attr.ArgOrder.Length];

                foreach (var i in attr.ArgOrder)
                {
                    if (parameters.Args[i]?.ToString() == null)
                    {
                        throw new ArgumentException("Null in labels... Something went wrong");
                    }

                    //TODO: for custom types apply custom logic here

                    args[i] = parameters.Args[i].ToString();
                }

                return args;
            }

            return Array.Empty<string>();
        }

        private static async Task WrapVoidAsync(Parameters parameters)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await ((Task)parameters.Target(parameters.Args)).ConfigureAwait(false);
            }
            finally
            {
                stopwatch.Stop();
                Console.Write($"Elapsed {stopwatch.Elapsed.TotalSeconds} seconds");

                //MetricsFactory.GetHistogram("TestWorkerTask_Run_duration", "fileType", "Duration of TestWorker.RunTask method in seconds")
                //    .WithLabels(fileType)
                //    .Observe(stopwatch.Elapsed.TotalSeconds);
            }
        }

        private struct Parameters
        {
            public Func<object[], object> Target;
            public object[] Args;
            public Attribute Attribute;
            public string MethodName;
            public string TypeName;
        }
    }
}
