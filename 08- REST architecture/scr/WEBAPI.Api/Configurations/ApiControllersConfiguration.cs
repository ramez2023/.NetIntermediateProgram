using Microsoft.AspNetCore.Mvc.Authorization;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;

namespace WEBAPI.Api.Configurations
{
    public static class ApiControllersConfiguration
    {
        public static void AddApiControllers(this IServiceCollection services)
        {
            services.AddMvcCore().AddApiExplorer();

            services.AddControllers(options =>
            {
                options.Filters.Add(new AllowAnonymousFilter());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Default;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }


        public static void AddWebApiEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", context =>
                {
                    return Task.Run(() => context.Response.Redirect("/swagger/index.html"));
                });
            });
        }
    }
}
