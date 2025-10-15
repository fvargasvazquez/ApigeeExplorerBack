using System.Text.Json.Serialization;

namespace ApigeeExplorer.ApiV2.Models.Entities
{
    /// <summary>
    /// Represents an Apigee target server
    /// </summary>
    public class TargetServer
    {
        [JsonPropertyName("host")]
        public string? Host { get; set; }

        [JsonPropertyName("api asociado")]
        public string? ApiAsociado { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("puerto")]
        public int? Puerto { get; set; }

        [JsonPropertyName("ssl activo")]
        public string? SslActivo { get; set; }

        [JsonPropertyName("target activo")]
        public bool? TargetActivo { get; set; }
    }
}