using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    public class Request
    {
        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("proto")]
        public string Proto { get; set; }

        [JsonPropertyName("headers")]
        public Dictionary<string, List<string>> Headers { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("raw")]
        public string Raw { get; set; }
    }
}
