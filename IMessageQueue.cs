namespace ServerMonitoringAndNotificationSystem
{
    public interface IMessageQueue
    {
        Task PublishAsync(string topic, object message);
    }
}