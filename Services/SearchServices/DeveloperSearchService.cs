using ApigeeExplorer.ApiV2.Models.Core;
using ApigeeExplorer.ApiV2.Models.Enriched;
using ApigeeExplorer.ApiV2.Models.Entities;
using ApigeeExplorer.ApiV2.Services.Interfaces;

namespace ApigeeExplorer.ApiV2.Services.SearchServices
{
    /// <summary>
    /// Service for searching developers
    /// </summary>
    public class DeveloperSearchService
    {
        private readonly IDataLoaderService _dataLoader;
        private readonly ILogger<DeveloperSearchService> _logger;

        public DeveloperSearchService(IDataLoaderService dataLoader, ILogger<DeveloperSearchService> logger)
        {
            _dataLoader = dataLoader;
            _logger = logger;
        }

        public List<SearchResult> SearchDevelopers(string environment, string searchTerm)
        {
            var results = new List<SearchResult>();
            var developers = _dataLoader.GetDevelopers(environment);
            var apps = _dataLoader.GetApps(environment);

            foreach (var dev in developers.Values)
            {
                if (IsMatch(dev, searchTerm))
                {
                    var enrichedApps = CreateEnrichedApps(dev.Apps, apps);
                    var details = CreateSearchResultDetails(dev, enrichedApps);

                    results.Add(new SearchResult
                    {
                        ComponentType = "Developer",
                        Id = dev.DeveloperId,
                        Name = $"{dev.Nombre} {dev.Apellidos}",
                        Environment = environment,
                        Component = dev,
                        Details = details
                    });
                }
            }

            _logger.LogInformation($"Found {results.Count} developers matching '{searchTerm}' in {environment}");
            return results;
        }

        private static bool IsMatch(Developer dev, string searchTerm)
        {
            return dev.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                   dev.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                   dev.Apellidos.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
        }

        private static List<EnrichedApp> CreateEnrichedApps(List<string> appNames, Dictionary<string, App> apps)
        {
            return appNames.Select(appName =>
            {
                var appDetails = apps.Values.FirstOrDefault(a => a.Name == appName);
                if (appDetails != null)
                {
                    var products = appDetails.Credentials
                        .SelectMany(c => c.Products.Select(p => p.ApiProduct))
                        .ToList();

                    return new EnrichedApp
                    {
                        Name = appName,
                        AppId = appDetails.AppId,
                        Username = appDetails.Username,
                        Products = products,
                        Status = appDetails.Status ?? "active"
                    };
                }

                return new EnrichedApp
                {
                    Name = appName,
                    AppId = "N/A",
                    Username = "N/A",
                    Products = new List<string>(),
                    Status = "unknown"
                };
            }).ToList();
        }

        private static SearchResultDetails CreateSearchResultDetails(Developer dev, List<EnrichedApp> enrichedApps)
        {
            return new SearchResultDetails
            {
                FullName = $"{dev.Nombre} {dev.Apellidos}",
                Email = dev.Email,
                Apps = dev.Apps,
                EnrichedApps = enrichedApps
            };
        }
    }
}