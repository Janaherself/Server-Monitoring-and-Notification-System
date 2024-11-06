namespace ServerMonitoringAndNotificationSystem.Interfaces
{
    public interface IMessageQueue
    {
        Task PublishAsync(string topic, object message);
    }
}