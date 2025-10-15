using ApigeeExplorer.ApiV2.Models.Entities;

namespace ApigeeExplorer.ApiV2.Services.Interfaces
{
    /// <summary>
    /// Interface for loading data from various sources
    /// </summary>
    public interface IDataLoaderService
    {
        /// <summary>
        /// Loads all data for the specified environment
        /// </summary>
        Task LoadEnvironmentDataAsync(string environment);

        /// <summary>
        /// Gets developers for the specified environment
        /// </summary>
        Dictionary<string, Developer> GetDevelopers(string environment);

        /// <summary>
        /// Gets apps for the specified environment
        /// </summary>
        Dictionary<string, App> GetApps(string environment);

        /// <summary>
        /// Gets products for the specified environment
        /// </summary>
        Dictionary<string, Product> GetProducts(string environment);

        /// <summary>
        /// Gets API proxies for the specified environment
        /// </summary>
        Dictionary<string, ApiProxy> GetApiProxies(string environment);

        /// <summary>
        /// Gets target servers for the specified environment
        /// </summary>
        Dictionary<string, TargetServer> GetTargetServers(string environment);

        /// <summary>
        /// Gets keystores for the specified environment
        /// </summary>
        Dictionary<string, Keystore> GetKeystores(string environment);

        /// <summary>
        /// Gets references for the specified environment
        /// </summary>
        Dictionary<string, Reference> GetReferences(string environment);

        /// <summary>
        /// Gets available environments
        /// </summary>
        List<string> GetEnvironments();
    }
}