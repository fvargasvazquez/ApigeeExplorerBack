namespace ApigeeExplorer.ApiV2.Models.Core
{
    /// <summary>
    /// Represents a search result for any Apigee component
    /// </summary>
    public class SearchResult
    {
        public string ComponentType { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public object Component { get; set; } = new();
        public SearchResultDetails Details { get; set; } = new();
    }
}