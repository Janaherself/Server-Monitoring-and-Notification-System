
namespace ServerMonitoringAndNotificationSystem
{
    public class RabbitMQService : IMessageQueue
    {
        public Task PublishAsync(string topic, object message)
        {
            throw new NotImplementedException();
        }
    }
}
