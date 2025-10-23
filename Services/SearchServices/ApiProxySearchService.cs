using ApigeeExplorer.ApiV2.Models.Core;
using ApigeeExplorer.ApiV2.Models.Enriched;
using ApigeeExplorer.ApiV2.Models.Entities;
using ApigeeExplorer.ApiV2.Services.Interfaces;

namespace ApigeeExplorer.ApiV2.Services.SearchServices
{
    /// <summary>
    /// Service for searching API proxies
    /// </summary>
    public class ApiProxySearchService
    {
        private readonly IDataLoaderService _dataLoader;
        private readonly ILogger<ApiProxySearchService> _logger;

        public ApiProxySearchService(IDataLoaderService dataLoader, ILogger<ApiProxySearchService> logger)
        {
            _dataLoader = dataLoader;
            _logger = logger;
        }

        public List<SearchResult> SearchApiProxies(string environment, string searchTerm)
        {
            var results = new List<SearchResult>();
            var apiProxies = _dataLoader.GetApiProxies(environment);

            foreach (var proxy in apiProxies.Values)
            {
                if (proxy.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    var enrichedEnvironments = CreateEnrichedEnvironments(proxy);
                    var allTargetServers = ExtractAllTargetServers(proxy);
                    var products = ExtractProducts(proxy);

                    var details = new SearchResultDetails
                    {
                        Environments = proxy.RevisionDeployada.Select(d => d.Ambiente).ToList(),
                        BasePaths = proxy.RevisionDeployada.Select(d => d.BasePath).Distinct().ToList(),
                        TargetServers = allTargetServers.ToList(),
                        Products = products,
                        EnrichedProxyEnvironments = enrichedEnvironments
                    };

                    results.Add(new SearchResult
                    {
                        ComponentType = "ApiProxy",
                        Id = proxy.Nombre,
                        Name = proxy.Nombre,
                        Environment = environment,
                        Component = proxy,
                        Details = details
                    });
                }
            }

            _logger.LogInformation($"Found {results.Count} API proxies matching '{searchTerm}' in {environment}");
            return results;
        }

        private List<EnrichedEnvironment> CreateEnrichedEnvironments(ApiProxy proxy)
        {
            var enrichedEnvironments = new List<EnrichedEnvironment>();

            foreach (var deployment in proxy.RevisionDeployada)
            {
                var targetServers = ExtractTargetServersForEnvironment(deployment.Target);
                var enrichedFlows = CreateEnrichedFlows(deployment.Flows);

                enrichedEnvironments.Add(new EnrichedEnvironment
                {
                    Ambiente = deployment.Ambiente,
                    BasePath = deployment.BasePath,
                    Flows = enrichedFlows,
                    TargetServers = targetServers,
                    Revision = deployment.Revision
                });
            }

            return enrichedEnvironments;
        }

        private List<string> ExtractTargetServersForEnvironment(List<Target> targets)
        {
            var targetServers = new List<string>();
            
            var targetsByName = targets
                .Where(t => !string.IsNullOrEmpty(t.TargetServer))
                .GroupBy(t => t.TargetServer)
                .ToList();

            foreach (var targetGroup in targetsByName)
            {
                var targetServer = targetGroup.Key;
                var targetList = targetGroup.ToList();

                // Find default and status targets
                var defaultTarget = targetList.FirstOrDefault(t => 
                    t.Nombre?.ToLower() == "default" || string.IsNullOrEmpty(t.Nombre));
                var statusTarget = targetList.FirstOrDefault(t => 
                    t.Nombre?.ToLower() == "status");

                // If we have both default and status pointing to the same server
                if (defaultTarget != null && statusTarget != null && 
                    defaultTarget.TargetServer == statusTarget.TargetServer)
                {
                    // Add only once without label since they're the same
                    targetServers.Add(targetServer);
                }
                else
                {
                    // Add each target with its specific name/purpose
                    foreach (var target in targetList)
                    {
                        var targetName = !string.IsNullOrEmpty(target.Nombre) 
                            ? $"{target.TargetServer} ({target.Nombre})"
                            : target.TargetServer;
                        targetServers.Add(targetName);
                    }
                }
            }

            return targetServers;
        }

        private List<EnrichedFlow> CreateEnrichedFlows(List<Flow>? flows)
        {
            var enrichedFlows = new List<EnrichedFlow>();

            if (flows != null)
            {
                foreach (var flow in flows)
                {
                    // Filter out common system flows
                    if (ShouldIncludeFlow(flow))
                    {
                        enrichedFlows.Add(new EnrichedFlow
                        {
                            Name = flow.Name,
                            Method = flow.Method,
                            Path = flow.Path
                        });
                    }
                }
            }

            return enrichedFlows;
        }

        private static bool ShouldIncludeFlow(Flow flow)
        {
            var flowName = flow.Name.ToLower();
            var flowPath = flow.Path.ToLower();

            // Exclude common system flows (removed ping and status to allow validation in frontend)
            var excludePatterns = new[]
            {
                "optionspreflight", "not-found", "notfound", 
                "options", "health", "healthcheck"
            };

            return !excludePatterns.Any(pattern => 
                flowName.Contains(pattern) || flowPath.Contains(pattern));
        }

        private HashSet<string> ExtractAllTargetServers(ApiProxy proxy)
        {
            var allTargetServers = new HashSet<string>();

            foreach (var deployment in proxy.RevisionDeployada)
            {
                var targetsByName = deployment.Target
                    .Where(t => !string.IsNullOrEmpty(t.TargetServer))
                    .GroupBy(t => t.TargetServer)
                    .ToList();

                foreach (var targetGroup in targetsByName)
                {
                    var targetServer = targetGroup.Key;
                    var targets = targetGroup.ToList();

                    // Find default and status targets
                    var defaultTarget = targets.FirstOrDefault(t => 
                        t.Nombre?.ToLower() == "default" || string.IsNullOrEmpty(t.Nombre));
                    var statusTarget = targets.FirstOrDefault(t => 
                        t.Nombre?.ToLower() == "status");

                    // If we have both default and status pointing to the same server
                    if (defaultTarget != null && statusTarget != null && 
                        defaultTarget.TargetServer == statusTarget.TargetServer)
                    {
                        // Add only once without label since they're the same
                        allTargetServers.Add(targetServer);
                    }
                    else
                    {
                        // Add each target with its specific name/purpose
                        foreach (var target in targets)
                        {
                            var targetName = !string.IsNullOrEmpty(target.Nombre) 
                                ? $"{target.TargetServer} ({target.Nombre})"
                                : target.TargetServer;
                            allTargetServers.Add(targetName);
                        }
                    }
                }
            }

            return allTargetServers;
        }

        private List<string> ExtractProducts(ApiProxy proxy)
        {
            if (string.IsNullOrEmpty(proxy.Productos))
            {
                return new List<string>();
            }

            return proxy.Productos
                .Split(',')
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .ToList();
        }
    }
}