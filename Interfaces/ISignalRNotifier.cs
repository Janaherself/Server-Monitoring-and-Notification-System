using ServerMonitoringAndNotificationSystem.ServerStatistics;

public interface ISignalRNotifier
{
    Task SendAnomalyAlertAsync(string message, ServerStatistics stats);
    Task SendHighUsageAlertAsync(string message, ServerStatistics stats);
}