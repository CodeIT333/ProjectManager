namespace API.Configurations
{
    public static class ConfigureAPI
    {
        public static IServiceCollection AddAPIServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
            });

            return services;
        }
    }
}
