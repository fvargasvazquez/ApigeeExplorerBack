using System.Text.Json.Serialization;

namespace ApigeeExplorer.ApiV2.Models.Entities
{
    /// <summary>
    /// Represents an Apigee API product
    /// </summary>
    public class Product
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("nombreDisplay")]
        public string NombreDisplay { get; set; } = string.Empty;

        [JsonPropertyName("apiResources")]
        public string ApiResources { get; set; } = string.Empty;

        [JsonPropertyName("apps")]
        public string? Apps { get; set; }

        [JsonPropertyName("proxies")]
        public List<string> Proxies { get; set; } = new();

        [JsonPropertyName("ambientes")]
        public string? Ambientes { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }
    }
}