using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Configuration;

namespace WEBAPI.Api.Configurations
{
    public static class CorsConfiguration
    {
        public static void AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            var list = new List<string>();
            configuration.GetSection("AllowedHosts").Bind(list);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .WithOrigins(list.ToArray())
                        .SetIsOriginAllowed(x => _ = true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });

            });
        }
    }
}
