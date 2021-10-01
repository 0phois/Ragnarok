using Ragnarok.AgentApi.Helpers;
using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    public class RequestParameters
    {
        [JsonPropertyName("tunnel_name")]
        public string TunnelName { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }
    }
}
