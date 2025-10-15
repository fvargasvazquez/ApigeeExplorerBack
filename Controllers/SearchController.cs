using Microsoft.AspNetCore.Mvc;
using ApigeeExplorer.ApiV2.Services.Interfaces;
using ApigeeExplorer.ApiV2.Models.Core;

namespace ApigeeExplorer.ApiV2.Controllers
{
    /// <summary>
    /// Controller for search operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly ILogger<SearchController> _logger;

        public SearchController(ISearchService searchService, ILogger<SearchController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        /// <summary>
        /// Gets available environments
        /// </summary>
        /// <returns>List of environment names</returns>
        [HttpGet("environments")]
        public ActionResult<List<string>> GetEnvironments()
        {
            try
            {
                var environments = _searchService.GetEnvironments();
                _logger.LogInformation($"Returning {environments.Count} environments");
                return Ok(environments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting environments");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Searches for components in the specified environment
        /// </summary>
        /// <param name="environment">Environment name (AWS, ONP)</param>
        /// <param name="searchTerm">Search term (minimum 2 characters)</param>
        /// <returns>List of search results</returns>
        [HttpGet("{environment}/{searchTerm}")]
        public async Task<ActionResult<List<SearchResult>>> Search(string environment, string searchTerm)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 2)
                {
                    return BadRequest(new { error = "Search term must be at least 2 characters" });
                }

                if (string.IsNullOrWhiteSpace(environment))
                {
                    return BadRequest(new { error = "Environment is required" });
                }

                _logger.LogInformation($"Search request: environment={environment}, searchTerm={searchTerm}");

                var results = await _searchService.SearchAsync(environment, searchTerm);

                _logger.LogInformation($"Returning {results.Count} results");
                return Ok(results);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid search parameters");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Search error");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}