using MongoDB.Driver;
using ServerMonitoringAndNotificationSystem.ServerStatistics;

public class MongoDbService : IMongoDbService
{
    private readonly IMongoCollection<ServerStatistics> _statisticsCollection;

    public MongoDbService(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _statisticsCollection = database.GetCollection<ServerStatistics>("ServerStatistics");
    }

    public async Task SaveStatisticsAsync(ServerStatistics stats)
    {
        await _statisticsCollection.InsertOneAsync(stats);
    }

    public async Task<ServerStatistics> GetPreviousStatisticsAsync(string serverIdentifier)
    {
        var filter = Builders<ServerStatistics>.Filter.Eq(s => s.ServerIdentifier, serverIdentifier);
        return await _statisticsCollection.Find(filter).SortByDescending(s => s.Timestamp).FirstOrDefaultAsync();
    }
}
