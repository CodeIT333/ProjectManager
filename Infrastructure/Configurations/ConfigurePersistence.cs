using Application.Programmers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Repositories.Programmers;

namespace Infrastructure.Configurations
{
    public static class ConfigurePersistence
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // register dbContext
            services.AddDbContext<ProjectManagerContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // register Repos
            services.AddScoped<IProgrammerRepository, ProgrammerRepository>();

            return services;
        }
    }
}
