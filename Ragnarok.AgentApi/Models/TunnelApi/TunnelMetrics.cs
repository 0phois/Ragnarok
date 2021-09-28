using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    public class TunnelMetrics
    {
        [JsonPropertyName("conns")]
        public TunnelConnectionMetrics ConnectionMetrics { get; set; }

        [JsonPropertyName("http")]
        public TunnelHttpMetrics HttpMetrics { get; set; }
    }
}
