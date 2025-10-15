using ApigeeExplorer.ApiV2.Models.Core;

namespace ApigeeExplorer.ApiV2.Services.Interfaces
{
    /// <summary>
    /// Interface for search operations
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Searches for components in the specified environment
        /// </summary>
        Task<List<SearchResult>> SearchAsync(string environment, string searchTerm);

        /// <summary>
        /// Gets available environments
        /// </summary>
        List<string> GetEnvironments();
    }
}