using System;
using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    public class RequestDetail
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("tunnel_name")]
        public string TunnelName { get; set; }

        [JsonPropertyName("remote_addr")]
        public string RemoteAddr { get; set; }

        [JsonPropertyName("start")]
        public DateTimeOffset Start { get; set; }

        [JsonPropertyName("duration")]
        public long Duration { get; set; }

        [JsonPropertyName("request")]
        public Request Request { get; set; }

        [JsonPropertyName("response")]
        public Response Response { get; set; }
    }
}
