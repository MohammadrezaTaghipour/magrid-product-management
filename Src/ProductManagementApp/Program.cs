using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProductManagementApp.Framework.Persistence;

namespace ProductManagementApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.AddControllers();
        builder.Services.AddMemoryCache(options =>
        {
            options.ExpirationScanFrequency = TimeSpan.FromMinutes(2);
        });


        builder.Services.AddDbContext<ProductManagementDbContext>(
            options =>
            {
                var connectionStr = configuration.GetConnectionString("dbConnection");
                options.UseSqlServer(connectionStr);
                var cache = builder.Services.BuildServiceProvider().GetRequiredService<IMemoryCache>();
                options.UseMemoryCache(cache);
            });


        var app = builder.Build();

        app.UseRouting();
        app.MapControllers();

        await app.RunAsync();
    }
}