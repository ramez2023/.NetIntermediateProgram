using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WEBAPI.IntegrationTests.Services;
using WEBAPI.IntegrationTests.Services.Interfaces;

namespace WEBAPI.IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            services.AddScoped<ICategoryTestService, CategoryTestService>();
            services.AddScoped<IProductTestService, ProductTestService>();

        });

        builder.UseEnvironment("Development");
    }
}