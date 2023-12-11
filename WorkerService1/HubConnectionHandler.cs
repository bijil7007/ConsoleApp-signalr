// HubConnectionHandler.cs
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public class HubConnectionHandler
{
    private readonly HubConnection _hubConnection;

    public HubConnectionHandler()
    {
        var httpClient = new HttpClient();
        var hubConnectionBuilder = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/myHub", options =>
            {
                options.HttpMessageHandlerFactory = (handler) =>
                {
                    if (handler is HttpClientHandler clientHandler)
                    {
                        // Uncomment the following line if you're using self-signed certificates
                        // clientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                    }
                    return handler;
                };
            })
            .WithAutomaticReconnect()
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Debug);
            });

        _hubConnection = hubConnectionBuilder.Build();
    }

    public async Task StartAsync()
    {
        await _hubConnection.StartAsync();
    }

    public async Task StopAsync()
    {
        await _hubConnection.StopAsync();
    }

    public async Task SendMessageAsync(string user, string message)
    {
        await _hubConnection.InvokeAsync("SendMessage", user, message);
    }
}
