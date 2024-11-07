﻿using Microsoft.Extensions.Configuration;
using ServerMonitoringAndNotificationSystem.Interfaces;
using System.Diagnostics;

namespace ServerMonitoringAndNotificationSystem.ServerStatistics
{
    public class ServerStatisticsCollector(IConfiguration config, IMessagePublisher messageQueue) : IServerStatisticsCollector
    {
        private readonly int _samplingIntervalSeconds = config.GetValue<int>("ServerStatisticsConfig:SamplingIntervalSeconds");
        private readonly string _serverIdentifier = config["ServerStatisticsConfig:ServerIdentifier"];
        private readonly IMessagePublisher _messageQueue = messageQueue;

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
                _messageQueue.Publish(topic, stats);

                await Task.Delay(_samplingIntervalSeconds * 1000, cancellationToken);
            }
        }
    }
}