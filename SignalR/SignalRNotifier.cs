using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using ServerMonitoringAndNotificationSystem.ServerStatistics;

public class SignalRNotifier : ISignalRNotifier, IHostedService
{
    private readonly HubConnection _connection;

    public SignalRNotifier(string signalRUrl)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(signalRUrl)
            .WithAutomaticReconnect()
            .Build();
    }

    public async Task SendAnomalyAlertAsync(string message, ServerStatistics stats)
    {
        try
        {
            _connection.On<string, object>("ReceiveAlert", (message, stats) =>
            {
                Console.WriteLine($"Alert received: {message},\n Data: {stats}");
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending anomaly alert: {ex.Message}");
        }
    }

    public async Task SendHighUsageAlertAsync(string message, ServerStatistics stats)
    {
        try
        {
            _connection.On<string, object>("ReceiveAlert", (message, stats) =>
            {
                Console.WriteLine($"Alert received: {message},\n Data: {stats}");
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending high usage alert: {ex.Message}");
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _connection.StartAsync();
        Console.WriteLine("Connected to SignalR Hub.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_connection != null)
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            Console.WriteLine("Disconnected from SignalR Hub.");
        }
    }
}
