namespace ApigeeExplorer.ApiV2.Models.Enriched
{
    /// <summary>
    /// Enriched product application model with developer information
    /// </summary>
    public class EnrichedProductApp
    {
        public string Name { get; set; } = string.Empty;
        public string AppId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string DeveloperName { get; set; } = string.Empty;
        public string DeveloperEmail { get; set; } = string.Empty;
    }
}