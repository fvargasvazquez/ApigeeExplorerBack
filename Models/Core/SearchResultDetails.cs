using ApigeeExplorer.ApiV2.Models.Enriched;

namespace ApigeeExplorer.ApiV2.Models.Core
{
    /// <summary>
    /// Contains detailed information for search results based on component type
    /// </summary>
    public class SearchResultDetails
    {
        // Developer Details
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public List<string>? Apps { get; set; }
        public List<EnrichedApp>? EnrichedApps { get; set; }

        // App Details
        public string? DeveloperName { get; set; }
        public string? Username { get; set; }
        public string? Status { get; set; }
        public List<string>? AppProducts { get; set; }

        // Product Details
        public string? ApiResources { get; set; }
        public List<string>? AssociatedApps { get; set; }
        public List<EnrichedProductApp>? EnrichedProductApps { get; set; }
        public List<string>? Proxies { get; set; }
        public List<EnrichedProxy>? EnrichedProxies { get; set; }

        // API Proxy Details
        public List<string>? Environments { get; set; }
        public List<string>? BasePaths { get; set; }
        public List<string>? TargetServers { get; set; }
        public List<string>? Products { get; set; }
        public List<EnrichedEnvironment>? EnrichedProxyEnvironments { get; set; }

        // Target Server Details
        public string? Host { get; set; }
        public int? Port { get; set; }
        public List<string>? AssociatedApis { get; set; }
        public Dictionary<string, List<string>>? ApisByEnvironment { get; set; }

        // Keystore Details
        public string? ExpirationDate { get; set; }
        public string? ValidationDate { get; set; }
        public string? IsValid { get; set; }

        // Reference Details
        public string? KeystoreReference { get; set; }
        public string? AliasName { get; set; }
    }
}