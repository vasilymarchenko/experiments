using WebApi1.Diagnostics;

namespace WebApi1.Services
{
    public class TestWorker
    {
        private static readonly string[] fileTypes = new[]
        {
            "jpg", "png", "gif", "pdf", "docx", "xls", "png", "tiff", "psd", "bmp"
        };

        public static void Run(string param1, string param2)
        {
            Console.WriteLine($"Flow started with '{param1}' and '{param2}'");

            var counter = MetricsFactory.GetCounter("TestWorker_Run");
            counter
                //.WithLabels(param1, param2)
                .Inc();

            for (int i = 0; i < fileTypes.Length; i++)
            {
                RunTask(fileTypes[i]);
            }
        }

        public static void RunTask(string fileType)
        {
            var delayMs = Random.Shared.Next(500);
            Console.WriteLine($"'{fileType}' is processing");
            Task.Delay(delayMs).Wait();
        }
    }
}
