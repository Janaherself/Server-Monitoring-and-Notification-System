using ServerMonitoringAndNotificationSystem.ServerStatistics;

public interface IMessageConsumer
{
    void Subscribe(string queueName, Action<string> onMessageReceived);
}