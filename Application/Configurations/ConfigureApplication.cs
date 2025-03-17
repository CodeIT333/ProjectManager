using Application.Programmers;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations
{
    public static class ConfigureApplication
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var Mappingconfig = TypeAdapterConfig.GlobalSettings;
            Mappingconfig.Scan(typeof(ConfigureApplication).Assembly); // scan mappings in the Application only
            services.AddSingleton(Mappingconfig); // live through the application lifetime

            services.AddScoped<ProgrammerService>();

            return services;
        }
    }
}
