using System.Text.Json.Serialization;

namespace ApigeeExplorer.ApiV2.Models.Entities
{
    /// <summary>
    /// Represents an Apigee keystore
    /// </summary>
    public class Keystore
    {
        [JsonPropertyName("alias")]
        public KeystoreAlias? Alias { get; set; }
    }

    /// <summary>
    /// Represents a keystore alias
    /// </summary>
    public class KeystoreAlias
    {
        [JsonPropertyName("informacion")]
        public List<KeystoreInfo> Informacion { get; set; } = new();
    }

    /// <summary>
    /// Represents keystore information
    /// </summary>
    public class KeystoreInfo
    {
        [JsonPropertyName("fecha de expiracion")]
        public string? FechaExpiracion { get; set; }

        [JsonPropertyName("es valido")]
        public string? EsValido { get; set; }

        [JsonPropertyName("Fecha de validacion")]
        public string? FechaValidacion { get; set; }

        [JsonPropertyName("Autorizador")]
        public string? Autorizador { get; set; }

        [JsonPropertyName("distinguido")]
        public string? Distinguido { get; set; }
    }
}