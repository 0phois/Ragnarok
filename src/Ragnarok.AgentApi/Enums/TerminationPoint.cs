using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi
{
    /// <summary>
    /// Confiure TLS terminination 
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TerminationPoint
    {
        /// <summary>
        /// default if --crt or --key is present
        /// </summary>
        edge,
        /// <summary>
        /// user specified
        /// </summary>
        agent,
        /// <summary>
        /// default
        /// </summary>
        upstream
    }
}
