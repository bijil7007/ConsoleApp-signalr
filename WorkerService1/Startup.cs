using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR();
        // Add other services as needed
    }

    public void Configure(IApplicationBuilder app)
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
    }
}
