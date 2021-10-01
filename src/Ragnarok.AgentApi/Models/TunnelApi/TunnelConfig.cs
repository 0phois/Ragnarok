using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    public class TunnelConfig
    {
        [JsonPropertyName("addr")]
        public string Address { get; set; }

        [JsonPropertyName("inspect")]
        public bool Inspect { get; set; }
    }
}
