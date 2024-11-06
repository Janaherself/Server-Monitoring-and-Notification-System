using ServerMonitoringAndNotificationSystem.ServerStatistics;

public interface IMongoDbService
{
    Task<ServerStatistics> GetLastStatisticsAsync(string serverIdentifier);
    Task SaveStatisticsAsync(ServerStatistics stats);
}