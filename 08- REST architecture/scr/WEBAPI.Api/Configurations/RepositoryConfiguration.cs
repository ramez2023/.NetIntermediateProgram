using WEBAPI.Infrastructure.Repositories;
using WEBAPI.Infrastructure.Repositories.Interfaces;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WEBAPI.Api.Configurations
{
    public static class RepositoryConfiguration
    {
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

        }
    }
}
