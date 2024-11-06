using Microsoft.Extensions.Configuration;

namespace ServerMonitoringAndNotificationSystem
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder().SetBasePath(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"))).AddJsonFile("appsettings.json").Build();

            var messageQueue = new RabbitMQService();
            var collector = new ServerStatisticsCollector(config, messageQueue);

            var cts = new CancellationTokenSource();
            await collector.StartAsync(cts.Token);
        }
    }
}