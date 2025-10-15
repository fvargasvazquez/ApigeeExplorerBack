using System.Text.Json.Serialization;

namespace ApigeeExplorer.ApiV2.Models.Entities
{
    /// <summary>
    /// Represents an Apigee reference
    /// </summary>
    public class Reference
    {
        [JsonPropertyName("tipo recurso")]
        public string? TipoRecurso { get; set; }

        [JsonPropertyName("ks referencia")]
        public string? KsReferencia { get; set; }

        [JsonPropertyName("nombre alias")]
        public string? NombreAlias { get; set; }
    }
}