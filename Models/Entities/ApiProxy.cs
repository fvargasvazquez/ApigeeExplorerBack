using System.Text.Json.Serialization;

namespace ApigeeExplorer.ApiV2.Models.Entities
{
    /// <summary>
    /// Represents an Apigee API proxy
    /// </summary>
    public class ApiProxy
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("productos")]
        public string Productos { get; set; } = string.Empty;

        [JsonPropertyName("revision-deployada")]
        public List<Deployment> RevisionDeployada { get; set; } = new();
    }

    /// <summary>
    /// Represents a deployment of an API proxy
    /// </summary>
    public class Deployment
    {
        [JsonPropertyName("ambiente")]
        public string Ambiente { get; set; } = string.Empty;

        [JsonPropertyName("basepath")]
        public string BasePath { get; set; } = string.Empty;

        [JsonPropertyName("flows")]
        public List<Flow>? Flows { get; set; }

        [JsonPropertyName("target")]
        public List<Target> Target { get; set; } = new();

        [JsonPropertyName("revision")]
        public string? Revision { get; set; }
    }

    /// <summary>
    /// Represents a flow in an API proxy
    /// </summary>
    public class Flow
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("condition")]
        public string Condition { get; set; } = string.Empty;

        [JsonPropertyName("productos")]
        public List<string> Productos { get; set; } = new();
    }

    /// <summary>
    /// Represents a target in an API proxy
    /// </summary>
    public class Target
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("target-server")]
        public string TargetServer { get; set; } = string.Empty;
    }
}