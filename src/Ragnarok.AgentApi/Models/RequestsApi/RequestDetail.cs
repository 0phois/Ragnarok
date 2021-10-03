using System;
using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Metadata and raw bytes of a captured request. The raw data is base64-encoded in the JSON response
    /// </summary>
    public class RequestDetail
    {
        /// <summary>
        /// Api endpoint
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Request id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the tunnel
        /// </summary>
        [JsonPropertyName("tunnel_name")]
        public string TunnelName { get; set; }

        /// <summary>
        /// Remote address
        /// </summary>
        [JsonPropertyName("remote_addr")]
        public string RemoteAddr { get; set; }

        /// <summary>
        /// Tunnel start
        /// </summary>
        [JsonPropertyName("start")]
        public DateTimeOffset Start { get; set; }

        /// <summary>
        /// Tunnel duration
        /// </summary>
        [JsonPropertyName("duration")]
        public long Duration { get; set; }

        /// <summary>
        /// Request details
        /// </summary>
        [JsonPropertyName("request")]
        public Request Request { get; set; }

        /// <summary>
        /// Request response
        /// </summary>
        [JsonPropertyName("response")]
        public Response Response { get; set; }
    }
}
