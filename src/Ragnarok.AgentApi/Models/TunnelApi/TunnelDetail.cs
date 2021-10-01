using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Reference: <see href="https://ngrok.com/docs#tunnel-detail">https://ngrok.com/docs#tunnel-detail</see>
    /// </summary>
    public class TunnelDetail
    {
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("uri")]
		public string URI { get; set; }

		[JsonPropertyName("public_url")]
		public string PublicURL { get; set; }

		[JsonPropertyName("proto")]
		public TunnelProtocol Proto { get; set; }

		[JsonPropertyName("config")]
		public TunnelConfig Config { get; set; }

		[JsonPropertyName("metrics")]
		public TunnelMetrics Metrics { get; set; }
	}
}
