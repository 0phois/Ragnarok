using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Tunnel configuration
    /// </summary>
    public class TunnelConfig
    {
        /// <summary>
        /// Local server address
        /// </summary>
        [JsonPropertyName("addr")]
        public string Address { get; set; }

        /// <summary>
        /// Enable/Disble recording HTTP request and response over your tunnels for inspection and replay
        /// </summary>
        [JsonPropertyName("inspect")]
        public bool Inspect { get; set; }
    }
}
