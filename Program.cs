using Microsoft.Extensions.Configuration;
using ServerMonitoringAndNotificationSystem.RabbitMQ;
using ServerMonitoringAndNotificationSystem.ServerStatistics;
using System.Configuration;

namespace ServerMonitoringAndNotificationSystem
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder().SetBasePath(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"))).AddJsonFile("appsettings.json").Build();

            var messageQueueService = new RabbitMQService();
            var mongoDbService = new MongoDbService(config["MongoDbConnection:ConnectionString"], config["MongoDbConnection:DatabaseName"]);
            var signalRNotifier = new SignalRNotifier(config["SignalRConfig:SignalRUrl"]);

            Console.WriteLine("Service running...");
            await Task.Delay(Timeout.Infinite);
        }
    }
}