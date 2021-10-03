using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi
{
    /// <summary>
    /// Tunnel protocol name
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TunnelProtocol
    {
        /// <summary>
        /// HyperText Transfer Protocol
        /// </summary>
        HTTP,
        /// <summary>
        /// HyperText Transfer Protocol Secure
        /// </summary>
        HTTPS,
        /// <summary>
        /// Transmission Control Protocol
        /// </summary>
        TCP,
        /// <summary>
        /// Transport Layer Security 
        /// </summary>
        TLS
    }
}
