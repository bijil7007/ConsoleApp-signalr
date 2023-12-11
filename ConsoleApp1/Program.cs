using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main()
    {
        BuildWebHost().Run();
    }

    public static IWebHost BuildWebHost() =>
        WebHost.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSignalR();
            })
            .Configure(app =>
            {
                app.UseRouting();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<ChatHub>("/chatHub");
                    endpoints.MapGet("/", async context =>
                    {
                        await context.Response.WriteAsync("SignalR Server is running.");
                    });
                });
            })
            .Build();
}
