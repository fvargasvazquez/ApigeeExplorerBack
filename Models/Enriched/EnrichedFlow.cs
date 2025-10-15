namespace ApigeeExplorer.ApiV2.Models.Enriched
{
    /// <summary>
    /// Enriched flow model representing API endpoints
    /// </summary>
    public class EnrichedFlow
    {
        public string Name { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
    }
}