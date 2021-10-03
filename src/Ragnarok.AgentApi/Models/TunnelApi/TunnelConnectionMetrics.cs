using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Connection rates and connection duration percentiles for tunnels
    /// </summary>
    public class TunnelConnectionMetrics : Metrics
    {
        /// <summary>
        /// Tunnel gauge
        /// </summary>
		[JsonPropertyName("gauge")]
		public int Gauge { get; set; }
	}
}