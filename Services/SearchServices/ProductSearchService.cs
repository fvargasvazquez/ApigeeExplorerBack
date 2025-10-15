using ApigeeExplorer.ApiV2.Models.Core;
using ApigeeExplorer.ApiV2.Models.Enriched;
using ApigeeExplorer.ApiV2.Models.Entities;
using ApigeeExplorer.ApiV2.Services.Interfaces;

namespace ApigeeExplorer.ApiV2.Services.SearchServices
{
    /// <summary>
    /// Service for searching products
    /// </summary>
    public class ProductSearchService
    {
        private readonly IDataLoaderService _dataLoader;
        private readonly ILogger<ProductSearchService> _logger;

        public ProductSearchService(IDataLoaderService dataLoader, ILogger<ProductSearchService> logger)
        {
            _dataLoader = dataLoader;
            _logger = logger;
        }

        public List<SearchResult> SearchProducts(string environment, string searchTerm)
        {
            var results = new List<SearchResult>();
            var products = _dataLoader.GetProducts(environment);
            var apps = _dataLoader.GetApps(environment);
            var developers = _dataLoader.GetDevelopers(environment);
            var apiProxies = _dataLoader.GetApiProxies(environment);

            foreach (var product in products.Values)
            {
                if (product.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    var (associatedApps, enrichedProductApps) = ProcessAssociatedApps(product, apps, developers);
                    var enrichedProxies = ProcessEnrichedProxies(product, apiProxies);

                    var details = new SearchResultDetails
                    {
                        ApiResources = product.ApiResources,
                        AssociatedApps = associatedApps,
                        EnrichedProductApps = enrichedProductApps,
                        Proxies = product.Proxies,
                        EnrichedProxies = enrichedProxies
                    };

                    results.Add(new SearchResult
                    {
                        ComponentType = "Product",
                        Id = product.Nombre,
                        Name = product.NombreDisplay,
                        Environment = environment,
                        Component = product,
                        Details = details
                    });
                }
            }

            _logger.LogInformation($"Found {results.Count} products matching '{searchTerm}' in {environment}");
            return results;
        }

        private static (List<string>, List<EnrichedProductApp>) ProcessAssociatedApps(
            Product product, 
            Dictionary<string, App> apps, 
            Dictionary<string, Developer> developers)
        {
            var associatedApps = new List<string>();
            var enrichedProductApps = new List<EnrichedProductApp>();

            if (!string.IsNullOrEmpty(product.Apps))
            {
                var appIds = product.Apps.Split(',').Select(a => a.Trim()).ToList();
                foreach (var appId in appIds)
                {
                    var app = apps.Values.FirstOrDefault(a => a.AppId == appId);
                    if (app != null)
                    {
                        associatedApps.Add(app.Name);
                        var developer = developers.Values.FirstOrDefault(d => d.DeveloperId == app.DeveloperId);
                        
                        enrichedProductApps.Add(new EnrichedProductApp
                        {
                            Name = app.Name,
                            AppId = app.AppId,
                            Username = app.Username,
                            Status = app.Status ?? "active",
                            DeveloperName = developer != null ? $"{developer.Nombre} {developer.Apellidos}" : "N/A",
                            DeveloperEmail = developer?.Email ?? "N/A"
                        });
                    }
                }
            }

            return (associatedApps, enrichedProductApps);
        }

        private static List<EnrichedProxy> ProcessEnrichedProxies(Product product, Dictionary<string, ApiProxy> apiProxies)
        {
            var enrichedProxies = new List<EnrichedProxy>();

            foreach (var proxyName in product.Proxies)
            {
                var proxyDetails = apiProxies.Values.FirstOrDefault(p => p.Nombre == proxyName);
                if (proxyDetails != null)
                {
                    var targetServers = new HashSet<string>();
                    foreach (var deployment in proxyDetails.RevisionDeployada)
                    {
                        foreach (var target in deployment.Target)
                        {
                            targetServers.Add(target.TargetServer);
                        }
                    }

                    enrichedProxies.Add(new EnrichedProxy
                    {
                        Name = proxyName,
                        Environments = proxyDetails.RevisionDeployada.Select(d => d.Ambiente).ToList(),
                        BasePaths = proxyDetails.RevisionDeployada.Select(d => d.BasePath).Distinct().ToList(),
                        TargetServers = targetServers.ToList()
                    });
                }
            }

            return enrichedProxies;
        }
    }
}