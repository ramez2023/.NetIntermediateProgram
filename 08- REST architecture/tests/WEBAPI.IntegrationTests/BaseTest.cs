using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace WEBAPI.IntegrationTests;

public class BaseTest : IAsyncLifetime
{       
    public ServiceProvider ServiceProvider = null!;
    public async Task InitializeAsync()
    {
        var builder = WebApplication.CreateBuilder();
        await Init(ServiceProvider, builder.Configuration);
    }

    private async Task Init(ServiceProvider serviceProvider,  IConfiguration configuration)
    {

    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}