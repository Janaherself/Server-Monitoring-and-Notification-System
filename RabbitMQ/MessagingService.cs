using ServerMonitoringAndNotificationSystem.Interfaces;

public class MessagingService
{
    private readonly IMessagePublisher _publisher;
    private readonly IMessageConsumer _consumer;

    public MessagingService(IMessagePublisher publisher, IMessageConsumer consumer)
    {
        _publisher = publisher;
        _consumer = consumer;
    }

    public void SendMessage(string message, string routingKey)
    {
        _publisher.Publish(message, routingKey);
    }

    public void StartListening(string queueName)
    {
        _consumer.Subscribe(queueName, message =>
        {
            Console.WriteLine($"Processed message: {message}");
        });
    }
}
