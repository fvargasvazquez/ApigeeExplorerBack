using System.Text.Json.Serialization;

namespace ApigeeExplorer.ApiV2.Models.Entities
{
    /// <summary>
    /// Represents an Apigee application
    /// </summary>
    public class App
    {
        [JsonPropertyName("appId")]
        public string AppId { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("developerId")]
        public string DeveloperId { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("credentials")]
        public List<Credential> Credentials { get; set; } = new();
    }

    /// <summary>
    /// Represents application credentials
    /// </summary>
    public class Credential
    {
        [JsonPropertyName("products")]
        public List<ProductReference> Products { get; set; } = new();
    }

    /// <summary>
    /// Represents a product reference in credentials
    /// </summary>
    public class ProductReference
    {
        [JsonPropertyName("apiproduct")]
        public string ApiProduct { get; set; } = string.Empty;
    }
}