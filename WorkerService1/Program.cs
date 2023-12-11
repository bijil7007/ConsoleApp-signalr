//namespace WorkerService1
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            IHost host = Host.CreateDefaultBuilder(args)
//                .ConfigureServices(services =>
//                {
//                    services.AddHostedService<Worker>();
//                })
//                .Build();

//            host.Run();
//        }
//    }
//}
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WorkerService1;
using static System.Net.WebRequestMethods;

class Program
{
    static void Main(string[] args)
    {
        // var serverUrl = "http://localhost:5001"; // Replace with your custom URL
        //CreateHostBuilder(args, serverUrl).Build().Run();
        //CreateHostBuilder(args).Build().Run();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        var serverUrl =configuration.GetValue<string>("SignalRClient:ServerUrl");
        CreateHostBuilder(args, serverUrl).Build().Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args, string serverUrl) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.ConfigureServices(services =>
            {
                services.AddSignalR();
            });

            webBuilder.UseUrls(serverUrl); // Make sure to configure your URLs correctly
            webBuilder.UseStartup<Startup>();
        });

    //public static IHostBuilder CreateHostBuilder(string[] args, string serverUrl) =>
    //   Host.CreateDefaultBuilder(args)
    //       .ConfigureServices(services =>
    //       {
    //           services.AddHostedService<Worker>(sp => new Worker(serverUrl));
    //       });
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<SignalRClientOptions>(hostContext.Configuration.GetSection("SignalRClient"));
                services.AddHostedService<Worker>();
            });
}
