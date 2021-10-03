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
		[JsonPropertyName("proto")]
		public TunnelProtocol Proto { get; set; }

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
