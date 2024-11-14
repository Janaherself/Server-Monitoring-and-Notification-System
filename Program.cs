using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerMonitoringAndNotificationSystem.Interfaces;

namespace ServerMonitoringAndNotificationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var config = new ConfigurationBuilder()
                        .SetBasePath(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\")))
                        .AddJsonFile("appsettings.json")
                        .Build();

                    services.AddSingleton(config);
                    services.AddRabbitMQClientLibrary(hostContext.Configuration);
                    services.AddScoped<MessagingService>();

                    services.AddScoped<MongoDbService>(sp => new MongoDbService(
                        config["MongoDbConnection:ConnectionString"],
                        config["MongoDbConnection:DatabaseName"]));

                    services.AddScoped<AnomalyDetectionConfig>(sp => config.GetSection("AnomalyDetectionConfig").Get<AnomalyDetectionConfig>());

                    services.AddScoped<MessageProcessor>(sp => new MessageProcessor(
                        sp.GetRequiredService<AnomalyDetectionConfig>(),
                        sp.GetRequiredService<MongoDbService>(),
                        sp.GetRequiredService<SignalRNotifier>()));

                    services.AddHostedService<SignalRNotifier>(sp => new SignalRNotifier(config["SignalRConfig:SignalRUrl"]));
                })
                .Build();

            var service = host.Services.GetRequiredService<MessagingService>();
            service.StartListening("MyQueue");

            host.Run();
        }
    }
}