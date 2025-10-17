using ApigeeExplorer.ApiV2.Models.Core;
using ApigeeExplorer.ApiV2.Models.Entities;
using ApigeeExplorer.ApiV2.Services.Interfaces;

namespace ApigeeExplorer.ApiV2.Services.SearchServices
{
    /// <summary>
    /// Service for searching applications
    /// </summary>
    public class AppSearchService
    {
        private readonly IDataLoaderService _dataLoader;
        private readonly ILogger<AppSearchService> _logger;

        public AppSearchService(IDataLoaderService dataLoader, ILogger<AppSearchService> logger)
        {
            _dataLoader = dataLoader;
            _logger = logger;
        }

        public List<SearchResult> SearchApps(string environment, string searchTerm)
        {
            var results = new List<SearchResult>();
            var apps = _dataLoader.GetApps(environment);
            var developers = _dataLoader.GetDevelopers(environment);

            foreach (var app in apps.Values)
            {
                if (IsMatch(app, searchTerm))
                {
                    var developer = developers.Values.FirstOrDefault(d => d.DeveloperId == app.DeveloperId);
                    var products = app.Credentials
                        .SelectMany(c => c.Products.Select(p => p.ApiProduct))
                        .ToList();

                    var details = new SearchResultDetails
                    {
                        DeveloperName = developer != null ? $"{developer.Nombre} {developer.Apellidos}" : "",
                        Username = app.Username,
                        Status = app.Status,
                        AppProducts = products
                    };

                    results.Add(new SearchResult
                    {
                        ComponentType = "App",
                        Id = app.AppId,
                        Name = app.Name,
                        Environment = environment,
                        Component = app,
                        Details = details
                    });
                }
            }

            _logger.LogInformation($"Found {results.Count} apps matching '{searchTerm}' in {environment}");
            return results;
        }

        private static bool IsMatch(App app, string searchTerm)
        {
            return app.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
        }
    }
}