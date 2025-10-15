namespace ApigeeExplorer.ApiV2.Models.Enriched
{
    /// <summary>
    /// Enriched environment model with flows and target servers
    /// </summary>
    public class EnrichedEnvironment
    {
        public string Ambiente { get; set; } = string.Empty;
        public string BasePath { get; set; } = string.Empty;
        public List<EnrichedFlow> Flows { get; set; } = new();
        public List<string> TargetServers { get; set; } = new();
        public string? Revision { get; set; }
    }
}