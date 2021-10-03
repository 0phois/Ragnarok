using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Response details for captured requests
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Response status message
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// Response status code
        /// </summary>
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        /// <summary>
        /// Response protocol
        /// </summary>
        [JsonPropertyName("proto")]
        public string Proto { get; set; }

        /// <summary>
        /// Response headers
        /// </summary>
        [JsonPropertyName("headers")]
        public Dictionary<string, List<string>> Headers { get; set; }

        /// <summary>
        /// Raw response data
        /// </summary>
        [JsonPropertyName("raw")]
        public string Raw { get; set; }
    }
}
