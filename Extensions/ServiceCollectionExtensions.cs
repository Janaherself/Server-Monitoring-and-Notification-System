using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerMonitoringAndNotificationSystem.Interfaces;
using ServerMonitoringAndNotificationSystem.RabbitMQ;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMQClientLibrary(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMQConfig = configuration.GetSection("RabbitMQ");
        string hostName = rabbitMQConfig["HostName"];
        string exchange = rabbitMQConfig["Exchange"];

        services.AddScoped<IMessagePublisher>(sp => new RabbitMQPublisher(hostName, exchange));
        services.AddScoped<IMessageConsumer>(sp => new RabbitMQConsumer(hostName));

        return services;
    }
}
