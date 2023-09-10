using System.Diagnostics;
using WebApi1.Diagnostics;

namespace WebApi1.Services
{
    public class TestWorker
    {
        private static readonly string[] _fileTypes = new[]
        {
            "jpg", "png", "gif", "pdf", "docx", "xls", "png", "tiff", "psd", "bmp"
        };

        private static readonly string[] _opTypes = new[]
        {
            "process image", "resising", "generate preview", "generate pdf", "generate thumbnail"
        };

        private static readonly Random _random = new Random();


        public static string GetRandomOperation()
        {
            int randomIndex = _random.Next(_opTypes.Length);
            return _opTypes[randomIndex];
        }

        public static void Run(string operation)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                Console.WriteLine($"Flow started operation '{operation}'");

                for (int i = 0; i < _fileTypes.Length; i++)
                {
                    RunTask(_fileTypes[i]);
                }
            }
            finally
            {
                stopwatch.Stop();

                // TODO: Not sure that caounter needed at all
                var counter = MetricsFactory.GetCounter("TestWorker_Run", "operation");
                counter
                    .WithLabels(operation)
                    .Inc();

                MetricsFactory.GetHistogram("TestWorker_Run_duration", "operation", "Duration of TestWorker.Run method in seconds")
                    .WithLabels(new[] { "fileType" }) // Replace "fileType" with the actual file type
                    .Observe(stopwatch.Elapsed.TotalSeconds);
            }
        }

        public static void RunTask(string fileType)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var delayMs = Random.Shared.Next(500);
                Console.WriteLine($"'{fileType}' is processing");
                Task.Delay(delayMs).Wait();
            }
            finally
            {
                stopwatch.Stop();

                // TODO: Not sure that caounter needed at all
                MetricsFactory.GetCounter("TestWorkerTask_Run", "fileType")
                    .WithLabels(fileType)
                    .Inc();

                MetricsFactory.GetHistogram("TestWorkerTask_Run_duration", "fileType", "Duration of TestWorker.RunTask method in seconds")
                    .WithLabels(fileType) 
                    .Observe(stopwatch.Elapsed.TotalSeconds);

                // NOTE: how to query it in Prometheus:
                // The histogram metrica contains mentrics '..._count' and '...sum' as well
                // Example:
                // avg by (fileType) (TestWorkerTask_Run_duration_sum / TestWorkerTask_Run_duration_count)
            }
        }
    }
}
