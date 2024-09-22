using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ProductManagementApp;
using ProductManagementApp.Framework.Persistence;
using Testcontainers.MsSql;

namespace ProductManagementAppTests.IntegrationTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
{
    private readonly MsSqlContainer _dbContainer;

    public IntegrationTestWebAppFactory()
    {
        _dbContainer = new MsSqlBuilder()
            .WithCleanUp(true)
            .Build();

        _dbContainer.StartAsync().Wait();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptorType = typeof(DbContextOptions<ProductManagementDbContext>);
            var descriptor = services.SingleOrDefault(s => s.ServiceType == descriptorType);
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }


            var connectionString = _dbContainer.GetConnectionString();
            services.AddDbContext<ProductManagementDbContext>((_, option) =>
            {
                option.UseSqlServer(connectionString);
                var cache = services.BuildServiceProvider().GetRequiredService<IMemoryCache>();
                option.UseMemoryCache(cache);
            });
            
            var dbContext = services.BuildServiceProvider().GetRequiredService<ProductManagementDbContext>();
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
        });
    }
}