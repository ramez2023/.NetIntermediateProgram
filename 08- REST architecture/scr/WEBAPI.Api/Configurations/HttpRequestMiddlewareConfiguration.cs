using System.Diagnostics.CodeAnalysis;
using WEBAPI.Api.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WEBAPI.Api.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class HttpRequestMiddlewareConfiguration
    {
        public static void AddHttpRequestMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionCatchMiddleware>();

        }

        public static void AddDeveloperHttpRequestMiddleware(this IApplicationBuilder app)
        {
            if (app.ApplicationServices.GetRequiredService<IHostEnvironment>().IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}   
