namespace ApigeeExplorer.ApiV2.Models.Enriched
{
    /// <summary>
    /// Enriched proxy model with environment and target server information
    /// </summary>
    public class EnrichedProxy
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Environments { get; set; } = new();
        public List<string> BasePaths { get; set; } = new();
        public List<string> TargetServers { get; set; } = new();
    }
}