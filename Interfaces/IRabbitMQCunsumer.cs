using ServerMonitoringAndNotificationSystem.ServerStatistics;

public interface IRabbitMQCunsumer
{
    Task OnMessageReceived(ServerStatistics stats);
    void SubscribeToServerStatistics();
}