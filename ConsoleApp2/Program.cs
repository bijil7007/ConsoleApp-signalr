using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("Enter your name: ");
        var user = Console.ReadLine();

        var hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/myHub")
            .Build();

        hubConnection.On<string, string>("ReceiveMessage", (sender, message) =>
        {
            Console.WriteLine($"{sender}: {message}");
        });

        await hubConnection.StartAsync();

        Console.WriteLine($"Connected to SignalR Server. Type 'exit' to quit.");

        while (true)
        {
            Console.Write("Enter message: ");
            var message = Console.ReadLine();

            if (message.ToLower() == "exit")
            {
                break;
            }

            await hubConnection.SendAsync("SendMessage", user, message);
        }

        await hubConnection.StopAsync();
    }
}
