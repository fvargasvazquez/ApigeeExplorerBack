using ApigeeExplorer.ApiV2.Services;
using ApigeeExplorer.ApiV2.Services.Interfaces;
using ApigeeExplorer.ApiV2.Services.SearchServices;

namespace ApigeeExplorer.ApiV2.Configuration
{
    /// <summary>
    /// Extension methods for configuring services
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds application services to the DI container
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Core services
            services.AddSingleton<IDataLoaderService, DataLoaderService>();
            services.AddScoped<ISearchService, SearchService>();

            // Search services
            services.AddScoped<DeveloperSearchService>();
            services.AddScoped<AppSearchService>();
            services.AddScoped<ProductSearchService>();
            services.AddScoped<ApiProxySearchService>();

            return services;
        }

        /// <summary>
        /// Adds CORS configuration
        /// </summary>
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            return services;
        }

        /// <summary>
        /// Adds JSON configuration
        /// </summary>
        public static IServiceCollection AddJsonConfiguration(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });

            return services;
        }
    }
}