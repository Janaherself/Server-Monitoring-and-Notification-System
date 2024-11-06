using Microsoft.AspNetCore.SignalR.Client;
using ServerMonitoringAndNotificationSystem.ServerStatistics;

public class SignalRNotifier(string signalRUrl) : ISignalRNotifier
{
    private readonly HubConnection _connection = new HubConnectionBuilder()
            .WithUrl(signalRUrl)
            .Build();

    public async Task SendAnomalyAlertAsync(ServerStatistics stats)
    {
        await _connection.InvokeAsync("SendAnomalyAlert", stats);
    }

    public async Task SendHighUsageAlertAsync(ServerStatistics stats)
    {
        await _connection.InvokeAsync("SendHighUsageAlert", stats);
    }
}
