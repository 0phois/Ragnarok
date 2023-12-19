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
        /// Default
        /// </summary>
        none,
        /// <summary>
        /// HyperText Transfer Protocol
        /// </summary>
        http,
        /// <summary>
        /// HyperText Transfer Protocol Secure
        /// </summary>
        https,
        /// <summary>
        /// Transmission Control Protocol
        /// </summary>
        tcp,
        /// <summary>
        /// Transport Layer Security 
        /// </summary>
        tls
    }
}
