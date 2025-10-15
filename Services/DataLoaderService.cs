using System.Text.Json;
using ApigeeExplorer.ApiV2.Models.Entities;
using ApigeeExplorer.ApiV2.Services.Interfaces;

namespace ApigeeExplorer.ApiV2.Services
{
    /// <summary>
    /// Service responsible for loading data from JSON files
    /// </summary>
    public class DataLoaderService : IDataLoaderService
    {
        private readonly Dictionary<string, Dictionary<string, Developer>> _developers = new();
        private readonly Dictionary<string, Dictionary<string, App>> _apps = new();
        private readonly Dictionary<string, Dictionary<string, Product>> _products = new();
        private readonly Dictionary<string, Dictionary<string, ApiProxy>> _apiProxies = new();
        private readonly Dictionary<string, Dictionary<string, TargetServer>> _targetServers = new();
        private readonly Dictionary<string, Dictionary<string, Keystore>> _keystores = new();
        private readonly Dictionary<string, Dictionary<string, Reference>> _references = new();
        private readonly ILogger<DataLoaderService> _logger;

        public DataLoaderService(ILogger<DataLoaderService> logger)
        {
            _logger = logger;
            InitializeData();
        }

        private void InitializeData()
        {
            _logger.LogInformation("Initializing data loader...");
            LoadEnvironmentDataAsync("AWS").Wait();
            LoadEnvironmentDataAsync("ONP").Wait();
        }

        public async Task LoadEnvironmentDataAsync(string environment)
        {
            var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "data", environment);
            _logger.LogInformation($"Loading data for {environment} from: {dataPath}");

            try
            {
                // Load all data types in parallel for better performance
                var tasks = new List<Task>
                {
                    LoadDevelopersAsync(environment, dataPath),
                    LoadAppsAsync(environment, dataPath),
                    LoadProductsAsync(environment, dataPath),
                    LoadApiProxiesAsync(environment, dataPath),
                    LoadTargetServersAsync(environment, dataPath),
                    LoadKeystoresAsync(environment, dataPath),
                    LoadReferencesAsync(environment, dataPath)
                };

                await Task.WhenAll(tasks);
                _logger.LogInformation($"Successfully loaded all data for {environment}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading data for environment {environment}");
                throw;
            }
        }

        private async Task LoadDevelopersAsync(string environment, string dataPath)
        {
            var developersData = await LoadJsonFileAsync<Dictionary<string, Developer>>(dataPath, "developers.json");
            _developers[environment] = developersData;
            _logger.LogInformation($"Loaded {developersData.Count} developers for {environment}");
        }

        private async Task LoadAppsAsync(string environment, string dataPath)
        {
            var appsData = await LoadJsonFileAsync<Dictionary<string, App>>(dataPath, "apps.json");
            _apps[environment] = appsData;
            _logger.LogInformation($"Loaded {appsData.Count} apps for {environment}");
        }

        private async Task LoadProductsAsync(string environment, string dataPath)
        {
            var productsData = await LoadJsonFileAsync<Dictionary<string, Product>>(dataPath, "products.json");
            _products[environment] = productsData;
            _logger.LogInformation($"Loaded {productsData.Count} products for {environment}");
        }

        private async Task LoadApiProxiesAsync(string environment, string dataPath)
        {
            var apiProxiesData = await LoadJsonFileAsync<Dictionary<string, ApiProxy>>(dataPath, "api_proxys.json");
            _apiProxies[environment] = apiProxiesData;
            _logger.LogInformation($"Loaded {apiProxiesData.Count} API proxies for {environment}");
        }

        private async Task LoadTargetServersAsync(string environment, string dataPath)
        {
            var targetServersData = await LoadJsonFileAsync<Dictionary<string, Dictionary<string, TargetServer>>>(dataPath, "target_servers.json");
            var flatServers = FlattenTargetServers(targetServersData);
            _targetServers[environment] = flatServers;
            _logger.LogInformation($"Loaded {flatServers.Count} target servers for {environment}");
        }

        private async Task LoadKeystoresAsync(string environment, string dataPath)
        {
            var keystoresData = await LoadJsonFileAsync<Dictionary<string, Dictionary<string, Keystore>>>(dataPath, "keystores.json");
            var flatKeystores = FlattenKeystores(keystoresData);
            _keystores[environment] = flatKeystores;
            _logger.LogInformation($"Loaded keystores for {environment}");
        }

        private async Task LoadReferencesAsync(string environment, string dataPath)
        {
            var referencesData = await LoadJsonFileAsync<Dictionary<string, Dictionary<string, Reference>>>(dataPath, "references.json");
            var flatReferences = FlattenReferences(referencesData);
            _references[environment] = flatReferences;
            _logger.LogInformation($"Loaded references for {environment}");
        }

        private async Task<T> LoadJsonFileAsync<T>(string dataPath, string filename) where T : new()
        {
            var filePath = Path.Combine(dataPath, filename);
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"File not found: {filePath}");
                return new T();
            }

            try
            {
                var fileContent = await File.ReadAllTextAsync(filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<T>(fileContent, options) ?? new T();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading {filename}");
                return new T();
            }
        }

        private static Dictionary<string, TargetServer> FlattenTargetServers(Dictionary<string, Dictionary<string, TargetServer>> nestedData)
        {
            var flatServers = new Dictionary<string, TargetServer>();
            foreach (var env in nestedData.Keys)
            {
                if (nestedData[env] != null)
                {
                    foreach (var serverName in nestedData[env].Keys)
                    {
                        if (!flatServers.ContainsKey(serverName))
                        {
                            // Create an enriched target server that includes environment info
                            var enrichedServer = new EnrichedTargetServer
                            {
                                Host = nestedData[env][serverName].Host,
                                ApiAsociado = nestedData[env][serverName].ApiAsociado,
                                Nombre = nestedData[env][serverName].Nombre,
                                Puerto = nestedData[env][serverName].Puerto,
                                SslActivo = nestedData[env][serverName].SslActivo,
                                TargetActivo = nestedData[env][serverName].TargetActivo,
                                Environments = new List<string> { env }
                            };
                            flatServers[serverName] = enrichedServer;
                        }
                        else
                        {
                            // If server already exists, add this environment to the list
                            if (flatServers[serverName] is EnrichedTargetServer existingEnriched)
                            {
                                if (!existingEnriched.Environments.Contains(env))
                                {
                                    existingEnriched.Environments.Add(env);
                                }
                            }
                        }
                    }
                }
            }
            return flatServers;
        }

        private static Dictionary<string, Keystore> FlattenKeystores(Dictionary<string, Dictionary<string, Keystore>> nestedData)
        {
            var flatKeystores = new Dictionary<string, Keystore>();
            foreach (var env in nestedData.Keys)
            {
                if (nestedData[env] != null)
                {
                    foreach (var keystoreName in nestedData[env].Keys)
                    {
                        if (!flatKeystores.ContainsKey(keystoreName))
                        {
                            flatKeystores[keystoreName] = nestedData[env][keystoreName];
                        }
                    }
                }
            }
            return flatKeystores;
        }

        private static Dictionary<string, Reference> FlattenReferences(Dictionary<string, Dictionary<string, Reference>> nestedData)
        {
            var flatReferences = new Dictionary<string, Reference>();
            foreach (var env in nestedData.Keys)
            {
                if (nestedData[env] != null)
                {
                    foreach (var refName in nestedData[env].Keys)
                    {
                        if (!flatReferences.ContainsKey(refName))
                        {
                            flatReferences[refName] = nestedData[env][refName];
                        }
                    }
                }
            }
            return flatReferences;
        }

        // Public getters
        public Dictionary<string, Developer> GetDevelopers(string environment) => 
            _developers.GetValueOrDefault(environment, new Dictionary<string, Developer>());

        public Dictionary<string, App> GetApps(string environment) => 
            _apps.GetValueOrDefault(environment, new Dictionary<string, App>());

        public Dictionary<string, Product> GetProducts(string environment) => 
            _products.GetValueOrDefault(environment, new Dictionary<string, Product>());

        public Dictionary<string, ApiProxy> GetApiProxies(string environment) => 
            _apiProxies.GetValueOrDefault(environment, new Dictionary<string, ApiProxy>());

        public Dictionary<string, TargetServer> GetTargetServers(string environment) => 
            _targetServers.GetValueOrDefault(environment, new Dictionary<string, TargetServer>());

        public Dictionary<string, Keystore> GetKeystores(string environment) => 
            _keystores.GetValueOrDefault(environment, new Dictionary<string, Keystore>());

        public Dictionary<string, Reference> GetReferences(string environment) => 
            _references.GetValueOrDefault(environment, new Dictionary<string, Reference>());

        public List<string> GetEnvironments() => new() { "AWS", "ONP" };
    }
}