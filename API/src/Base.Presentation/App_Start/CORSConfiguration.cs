namespace TaskingSystem.Presentation.App_Start
{
    public static class CORSConfiguration
    {
        public static IServiceCollection AddCorsPolicy(
            this IServiceCollection services,
            string policyName,
            string[] allowedOrigins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName,
                    builder => builder.WithOrigins(allowedOrigins)
                             .AllowAnyMethod()
                             .AllowAnyHeader()
                             .AllowCredentials());
            });

            return services;
        }
    }
}
