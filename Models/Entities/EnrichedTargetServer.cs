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
    }
}