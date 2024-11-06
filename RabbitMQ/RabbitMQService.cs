using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ServerMonitoringAndNotificationSystem.Interfaces;

namespace ServerMonitoringAndNotificationSystem.RabbitMQ
{
    public class RabbitMQService : IMessageQueue
    {
        public async Task PublishAsync(string topic, object message)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "default",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

            var messageBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageBody);

            channel.BasicPublish(exchange: "",
                        routingKey: "default",
                        basicProperties: null,
                        body: body);

            Console.WriteLine($"{topic} Published a message: {message}");
        }
    }
}
