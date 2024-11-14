using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ServerMonitoringAndNotificationSystem.Interfaces;

namespace ServerMonitoringAndNotificationSystem.RabbitMQ
{
    public class RabbitMQPublisher : IMessagePublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchange;

        public RabbitMQPublisher(string hostName, string exchange)
        {
            _exchange = exchange;

            var factory = new ConnectionFactory { HostName = hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Direct);
        }

        public void Publish(string routingKey, object message)
        {
            var messageBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageBody);

            _channel.BasicPublish(exchange: _exchange,
                        routingKey: routingKey,
                        basicProperties: null,
                        body: body);

            Console.WriteLine($"{routingKey} Published a message: {message}");
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
