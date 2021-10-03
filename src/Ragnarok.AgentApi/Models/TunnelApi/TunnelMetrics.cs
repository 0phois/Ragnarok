using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Metrics information about the running ngrok process
    /// </summary>
    public class TunnelMetrics
    {
        /// <summary>
        /// Connection rates and connection duration percentiles for tunnels
        /// </summary>
        [JsonPropertyName("conns")]
        public TunnelConnectionMetrics ConnectionMetrics { get; set; }

        /// <summary>
        ///  Http request rates and http response duration percentiles
        /// </summary>
        [JsonPropertyName("http")]
        public TunnelHttpMetrics HttpMetrics { get; set; }
    }
}
