using ServerMonitoringAndNotificationSystem.ServerStatistics;

public interface ISignalRNotifier
{
    Task SendAnomalyAlertAsync(ServerStatistics stats);
    Task SendHighUsageAlertAsync(ServerStatistics stats);
}