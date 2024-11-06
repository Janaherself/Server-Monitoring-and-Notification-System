using System.Text;
using System.Text.Json;
using MongoDB.Bson.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServerMonitoringAndNotificationSystem.ServerStatistics;

public class RabbitMQCunsumer : IRabbitMQCunsumer
{
    public void SubscribeToServerStatistics()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "ServerStatistics",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var stats = JsonSerializer.Deserialize<ServerStatistics>(message);
            Console.WriteLine($"Message received: \n" +
                $"{stats}");
        };

        channel.BasicConsume(queue: "ServerStatistics",
                    autoAck: true,
                    consumer: consumer);

        Console.ReadKey();
    }

    public async Task OnMessageReceived(ServerStatistics stats)
    {
        throw new NotImplementedException();
    }
}