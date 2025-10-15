using System.Text.Json.Serialization;

namespace ApigeeExplorer.ApiV2.Models.Entities
{
    /// <summary>
    /// Enriched target server that includes environment information
    /// </summary>
    public class EnrichedTargetServer : TargetServer
    {
        /// <summary>
        /// List of environments where this target server is deployed
        /// </summary>
        public List<string> Environments { get; set; } = new();

        /// <summary>
        /// APIs associated with this target server grouped by environment
        /// </summary>
        public Dictionary<string, List<string>> ApisByEnvironment { get; set; } = new();
    }
}