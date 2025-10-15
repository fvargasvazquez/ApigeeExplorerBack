namespace ApigeeExplorer.ApiV2.Models.Enriched
{
    /// <summary>
    /// Enriched application model with additional computed properties
    /// </summary>
    public class EnrichedApp
    {
        public string Name { get; set; } = string.Empty;
        public string AppId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public List<string> Products { get; set; } = new();
        public string Status { get; set; } = string.Empty;
    }
}