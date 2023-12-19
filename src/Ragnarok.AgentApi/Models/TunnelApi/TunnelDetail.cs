using System;
using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// status and metrics about the named running tunnel <br/>
    /// Reference: <see href="https://ngrok.com/docs#tunnel-detail"/>
    /// </summary>
    public class TunnelDetail
    {
        /// <summary>
        /// Name of the tunnel
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Api endpoint
        /// </summary>
        [JsonPropertyName("uri")]
        public string URI { get; set; }

        /// <summary>
        /// Publicly exposed ngrok url
        /// </summary>
        [JsonPropertyName("public_url")]
        public string PublicURL { get; set; }

        /// <summary>
        /// Tunnel protocol
        /// </summary>
        [JsonIgnore]
        public TunnelProtocol Protocol { get; set; }

        /// <summary>
        /// Tunnel protocol
        /// </summary>
        [JsonPropertyName("proto")]
        public string Proto
        {
            get
            {
                if (Protocol == TunnelProtocol.none) return null;

                return Protocol.ToString();
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    Protocol = TunnelProtocol.none;
                else
                    Protocol = Enum.Parse<TunnelProtocol>(value);
            }
        }

        /// <summary>
        /// Tunnel configuration
        /// </summary>
        [JsonPropertyName("config")]
        public TunnelConfig Config { get; set; }

        /// <summary>
        /// Tunnel traffic metrics
        /// </summary>
        [JsonPropertyName("metrics")]
        public TunnelMetrics Metrics { get; set; }
    }
}
