using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

public class Worker : BackgroundService
{
    private readonly string _serverUrl=string.Empty;

    public Worker(string url)
    {
        _serverUrl = url;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var host = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(ParseCustomUrl(_serverUrl), listenOptions => listenOptions.UseHttps()); //
                });

                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<MyHub>("/myHub");
                        endpoints.MapGet("/", async context =>
                        {
                            await context.Response.WriteAsync("SignalR Server is running.");
                        });
                    });
                });
            })
            .Build();

        await host.RunAsync(stoppingToken);
    }
    //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //{
    //    using var host = new WebHostBuilder()
    //        .ConfigureKestrel(options =>
    //        {
    //            options.ListenAnyIP(ParseCustomUrl(_serverUrl), listenOptions => listenOptions.UseHttps());
    //        })
    //        .Configure(app =>
    //        {
    //            app.UseRouting();
    //            app.UseEndpoints(endpoints =>
    //            {
    //                endpoints.MapHub<MyHub>("/myHub");
    //                endpoints.MapGet("/", async context =>
    //                {
    //                    await context.Response.WriteAsync("SignalR Server is running.");
    //                });
    //            });
    //        })
    //        .Build();

    //    await host.RunAsync(stoppingToken);
    //}

    private int ParseCustomUrl(string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return uri.Port;
        }

        throw new ArgumentException("Invalid URL format", nameof(url));
    }
}
