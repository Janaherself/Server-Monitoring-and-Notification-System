using Microsoft.Extensions.Configuration;
using ServerMonitoringAndNotificationSystem;
using System.Diagnostics;

public class ServerStatisticsCollector(IConfiguration config, IMessageQueue messageQueue)
{
    private readonly int _samplingIntervalSeconds = config.GetValue<int>("ServerStatisticsConfig:SamplingIntervalSeconds");
    private readonly string _serverIdentifier = config["ServerStatisticsConfig:ServerIdentifier"];
    private readonly IMessageQueue _messageQueue = messageQueue;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
        var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        while (!cancellationToken.IsCancellationRequested)
        {
            var stats = new ServerStatistics
            {
                MemoryUsage = GC.GetTotalMemory(false) / 1024 / 1024,
                AvailableMemory = memoryCounter.NextValue(),
                CpuUsage = cpuCounter.NextValue(),
                Timestamp = DateTime.UtcNow
            };

            string topic = $"ServerStatistics.{_serverIdentifier}";
            await _messageQueue.PublishAsync(topic, stats);

            await Task.Delay(_samplingIntervalSeconds * 1000, cancellationToken);
        }
    }
}
