using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    public class TunnelConnectionMetrics : Metrics
    {
		[JsonPropertyName("gauge")]
		public int Gauge { get; set; }
	}
}