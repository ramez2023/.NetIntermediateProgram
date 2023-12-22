using WEBAPI.Service.AutoMapper;

namespace WEBAPI.Api.Configurations
{
    public static class MapperConfiguration
    {
        public static void AddMapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(config =>
            {
                config.AddProfile<ProductProfile>();
                config.AddProfile<CategoryProfile>();
            });
        }
    }
}
