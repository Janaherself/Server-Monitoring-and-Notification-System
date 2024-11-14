namespace ServerMonitoringAndNotificationSystem.Interfaces
{
    public interface IServerStatisticsCollector
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}