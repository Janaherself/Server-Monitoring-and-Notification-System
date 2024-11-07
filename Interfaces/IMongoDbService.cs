using ServerMonitoringAndNotificationSystem.ServerStatistics;

public interface IMongoDbService
{
    Task<ServerStatistics> GetPreviousStatisticsAsync(string serverIdentifier);
    Task SaveStatisticsAsync(ServerStatistics stats);
}