using ApigeeExplorer.ApiV2.Models.Core;
using ApigeeExplorer.ApiV2.Models.Entities;
using ApigeeExplorer.ApiV2.Services.Interfaces;
using ApigeeExplorer.ApiV2.Services.SearchServices;

namespace ApigeeExplorer.ApiV2.Services
{
    /// <summary>
    /// Main search service that coordinates all component-specific search services
    /// </summary>
    public class SearchService : ISearchService
    {
        private readonly IDataLoaderService _dataLoader;
        private readonly DeveloperSearchService _developerSearch;
        private readonly AppSearchService _appSearch;
        private readonly ProductSearchService _productSearch;
        private readonly ApiProxySearchService _apiProxySearch;
        private readonly ILogger<SearchService> _logger;

        public SearchService(
            IDataLoaderService dataLoader,
            DeveloperSearchService developerSearch,
            AppSearchService appSearch,
            ProductSearchService productSearch,
            ApiProxySearchService apiProxySearch,
            ILogger<SearchService> logger)
        {
            _dataLoader = dataLoader;
            _developerSearch = developerSearch;
            _appSearch = appSearch;
            _productSearch = productSearch;
            _apiProxySearch = apiProxySearch;
            _logger = logger;
        }

        public async Task<List<SearchResult>> SearchAsync(string environment, string searchTerm)
        {
            var results = new List<SearchResult>();
            var normalizedEnvironment = environment.ToUpper();

            _logger.LogInformation($"Starting search in {normalizedEnvironment} for: {searchTerm}");

            try
            {
                // Execute all searches in parallel for better performance
                var searchTasks = new List<Task<List<SearchResult>>>
                {
                    Task.Run(() => _developerSearch.SearchDevelopers(normalizedEnvironment, searchTerm)),
                    Task.Run(() => _appSearch.SearchApps(normalizedEnvironment, searchTerm)),
                    Task.Run(() => _productSearch.SearchProducts(normalizedEnvironment, searchTerm)),
                    Task.Run(() => _apiProxySearch.SearchApiProxies(normalizedEnvironment, searchTerm)),
                    Task.Run(() => SearchTargetServers(normalizedEnvironment, searchTerm)),
                    Task.Run(() => SearchKeystores(normalizedEnvironment, searchTerm)),
                    Task.Run(() => SearchReferences(normalizedEnvironment, searchTerm))
                };

                var searchResults = await Task.WhenAll(searchTasks);
                
                // Combine all results
                foreach (var resultSet in searchResults)
                {
                    results.AddRange(resultSet);
                }

                _logger.LogInformation($"Search completed. Found {results.Count} total results");
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during search in {normalizedEnvironment} for '{searchTerm}'");
                throw;
            }
        }

        public List<string> GetEnvironments()
        {
            return _dataLoader.GetEnvironments();
        }

        // TODO: Move remaining search methods to separate service classes

        private List<SearchResult> SearchTargetServers(string environment, string searchTerm)
        {
            var results = new List<SearchResult>();
            var targetServers = _dataLoader.GetTargetServers(environment);

            foreach (var (serverName, server) in targetServers)
            {
                if (serverName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    var associatedApis = new List<string>();
                    var environments = new List<string>();
                    Dictionary<string, List<string>>? apisByEnvironment = null;

                    if (server is EnrichedTargetServer enrichedServer)
                    {
                        environments = enrichedServer.Environments;
                        apisByEnvironment = enrichedServer.ApisByEnvironment;
                        
                        // For backward compatibility, still populate the flat list
                        associatedApis = enrichedServer.ApisByEnvironment.Values
                            .SelectMany(apis => apis)
                            .Distinct()
                            .ToList();
                    }
                    else if (!string.IsNullOrEmpty(server.ApiAsociado))
                    {
                        associatedApis = server.ApiAsociado.Split(',').Select(api => api.Trim()).ToList();
                    }

                    var details = new SearchResultDetails
                    {
                        Host = server.Host ?? "N/A",
                        Port = server.Puerto,
                        Environments = environments,
                        AssociatedApis = associatedApis,
                        ApisByEnvironment = apisByEnvironment
                    };

                    results.Add(new SearchResult
                    {
                        ComponentType = "TargetServer",
                        Id = serverName,
                        Name = serverName,
                        Environment = environment,
                        Component = server,
                        Details = details
                    });
                }
            }

            return results;
        }

        private List<SearchResult> SearchKeystores(string environment, string searchTerm)
        {
            var results = new List<SearchResult>();
            var keystores = _dataLoader.GetKeystores(environment);

            foreach (var (keystoreName, keystoreData) in keystores)
            {
                if (keystoreName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    var details = new SearchResultDetails();

                    if (keystoreData?.Alias?.Informacion != null && keystoreData.Alias.Informacion.Count > 0)
                    {
                        var info = keystoreData.Alias.Informacion[0];
                        details.ExpirationDate = info.FechaExpiracion ?? "N/A";
                        details.ValidationDate = info.FechaValidacion ?? "N/A";
                        details.IsValid = info.EsValido ?? "N/A";
                    }

                    results.Add(new SearchResult
                    {
                        ComponentType = "Keystore",
                        Id = keystoreName,
                        Name = keystoreName,
                        Environment = environment,
                        Component = keystoreData,
                        Details = details
                    });
                }
            }

            return results;
        }

        private List<SearchResult> SearchReferences(string environment, string searchTerm)
        {
            var results = new List<SearchResult>();
            var references = _dataLoader.GetReferences(environment);

            foreach (var (refName, refData) in references)
            {
                if (refName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    var details = new SearchResultDetails
                    {
                        KeystoreReference = refData.KsReferencia ?? "N/A",
                        AliasName = refData.NombreAlias ?? "N/A"
                    };

                    results.Add(new SearchResult
                    {
                        ComponentType = "Reference",
                        Id = refName,
                        Name = refName,
                        Environment = environment,
                        Component = refData,
                        Details = details
                    });
                }
            }

            return results;
        }
    }
}