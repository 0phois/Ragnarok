using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    public class TunnelList
    {
		[JsonPropertyName("tunnels")]
		public IEnumerable<TunnelDetail> Tunnels { get; set; }

		[JsonPropertyName("uri")]
		public string Uri { get; set; }
	}
}
