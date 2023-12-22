using FluentValidation;
using WEBAPI.Common.ViewModels;
using WEBAPI.Service.Services;
using WEBAPI.Service.Services.Interfaces;
using WEBAPI.Service.Validators;
using WEBAPI.Service.ViewModels;

namespace WEBAPI.Api.Configurations
{
    public static class ServiceConfiguration
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Core Services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Services
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

        }
    }
}
