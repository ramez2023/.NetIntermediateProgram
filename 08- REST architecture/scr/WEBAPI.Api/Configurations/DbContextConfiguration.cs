using WEBAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;
using WEBAPI.Infrastructure.SeedData;

namespace WEBAPI.Api.Configurations
{
    public static class DbContextConfiguration
    {
        public const bool InMemoryDatabase = false;

        public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            if (InMemoryDatabase)
            {
                services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                var connectionString = configuration["ConnectionString"];
                services.AddDbContext<ApplicationContext>
                (options =>
                {
                    options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(ApplicationContext).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });

                    options.EnableSensitiveDataLogging();
                });
            }
        }

        public static void AddDbMigrateAndSeedData(this IApplicationBuilder app, IConfiguration configuration)
        {
            var serviceProvider = app.ApplicationServices.GetRequiredService<IServiceProvider>();

            using (var scope = serviceProvider.CreateScope())
            {
                var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                if (InMemoryDatabase)
                {
                    DbInitializer.Initialize(applicationContext);
                }
                else
                {
                    applicationContext!.GetService<IMigrator>().Migrate();
                }
            }
        }
    }
}
