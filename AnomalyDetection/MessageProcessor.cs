using ServerMonitoringAndNotificationSystem.ServerStatistics;

public class MessageProcessor
{
    private readonly MongoDbService _mongoDBService;
    private readonly AnomalyDetectionConfig _anomalyConfig;
    private readonly SignalRNotifier _signalRNotifier;

    public MessageProcessor(
        AnomalyDetectionConfig config,
        MongoDbService mongoDBService,
        SignalRNotifier signalRNotifier)
    {
        _anomalyConfig = config;
        _mongoDBService = mongoDBService;
        _signalRNotifier = signalRNotifier;
    }

    public async Task ProcessMessageAsync(ServerStatistics currentStats)
    {
        var previousStats = await _mongoDBService.GetPreviousStatisticsAsync(currentStats.ServerIdentifier);

        bool isAnomaly = DetectAnomaly(currentStats, previousStats);

        if (isAnomaly)
        {
            await _signalRNotifier.SendAnomalyAlertAsync("Anomaly Alert: Sudden change in stats!", currentStats);
        }

        if (IsHighUsage(currentStats))
        {
            await _signalRNotifier.SendHighUsageAlertAsync("High Usage Alert: Memory or CPU usage exceeds threshold.", currentStats);
        }
    }

    private bool DetectAnomaly(ServerStatistics currentStats, ServerStatistics previousStats)
    {
        if (previousStats == null)
        {
            return false;
        }
        else
        {
            bool memoryAnomaly = currentStats.MemoryUsage > (previousStats.MemoryUsage * (1 + _anomalyConfig.MemoryUsageAnomalyThresholdPercentage));
            bool cpuAnomaly = currentStats.CpuUsage > (previousStats.CpuUsage * (1 + _anomalyConfig.CpuUsageAnomalyThresholdPercentage));

            return memoryAnomaly || cpuAnomaly;
        }
    }

    private bool IsHighUsage(ServerStatistics currentStats)
    {
        bool memoryHighUsage = (currentStats.MemoryUsage / (currentStats.MemoryUsage + currentStats.AvailableMemory)) > _anomalyConfig.MemoryUsageThresholdPercentage;
        bool cpuHighUsage = currentStats.CpuUsage > _anomalyConfig.CpuUsageThresholdPercentage;

        return memoryHighUsage || cpuHighUsage;
    }
}
