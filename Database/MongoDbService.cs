using MongoDB.Driver;
using ServerMonitoringAndNotificationSystem.ServerStatistics;

public class MongoDbService : IMongoDbService
{
    private readonly IMongoCollection<ServerStatistics> _statisticsCollection;

    public MongoDbService(string connectionString)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("ServerStatsDB");
        _statisticsCollection = database.GetCollection<ServerStatistics>("ServerStatistics");
    }

    public async Task SaveStatisticsAsync(ServerStatistics stats)
    {
        await _statisticsCollection.InsertOneAsync(stats);
    }

    public async Task<ServerStatistics> GetLastStatisticsAsync(string serverIdentifier)
    {
        var filter = Builders<ServerStatistics>.Filter.Eq(s => s.ServerIdentifier, serverIdentifier);
        return await _statisticsCollection.Find(filter).SortByDescending(s => s.Timestamp).FirstOrDefaultAsync();
    }
}
