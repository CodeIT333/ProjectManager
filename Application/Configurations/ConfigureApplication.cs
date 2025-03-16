using Application.Programmers;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations
{
    public static class ConfigureApplication
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ProgrammerService>();

            return services;
        }
    }
}
