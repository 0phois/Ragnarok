using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    public class CapturedRequest
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("requests")]
        public List<RequestDetail> Requests { get; set; }
    }
}
