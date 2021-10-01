using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    public class Response
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("proto")]
        public string Proto { get; set; }

        [JsonPropertyName("headers")]
        public Dictionary<string, List<string>> Headers { get; set; }

        [JsonPropertyName("raw")]
        public string Raw { get; set; }
    }
}
