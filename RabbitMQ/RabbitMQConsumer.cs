using System.Data.Common;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using MongoDB.Bson.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServerMonitoringAndNotificationSystem.ServerStatistics;

public class RabbitMQConsumer : IMessageConsumer, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQConsumer(string hostName)
    {
        var factory = new ConnectionFactory { HostName = hostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Subscribe(string queueName, Action<string> onMessageReceived)
    {
        _channel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var stats = JsonSerializer.Deserialize<ServerStatistics>(message);
            onMessageReceived(message);

            Console.WriteLine($"Message received: \n{stats}");
        };

        _channel.BasicConsume(queue: "ServerStatistics",
                    autoAck: true,
                    consumer: consumer);
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}