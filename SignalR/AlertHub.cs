using Microsoft.AspNetCore.SignalR;
using ServerMonitoringAndNotificationSystem.ServerStatistics;

public class AlertHub : Hub
{
    public async Task SendAnomalyAlert(string message, ServerStatistics stats)
    {
        await Clients.All.SendAsync("ReceiveAnomalyAlert", message, stats);
    }

    public async Task SendHighUsageAlert(string message, ServerStatistics stats)
    {
        await Clients.All.SendAsync("ReceiveHighUsageAlert", message, stats);
    }
}