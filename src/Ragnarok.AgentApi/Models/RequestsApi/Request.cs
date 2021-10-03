using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Request details for captured requests
    /// </summary>
    public class Request
    {
        /// <summary>
        /// HTTP request method
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; }

        /// <summary>
        /// Request protocol
        /// </summary>
        [JsonPropertyName("proto")]
        public string Proto { get; set; }

        /// <summary>
        /// Request headers
        /// </summary>
        [JsonPropertyName("headers")]
        public Dictionary<string, List<string>> Headers { get; set; }

        /// <summary>
        /// Request uri
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        /// <summary>
        ///  Raw bytes of a captured request
        /// </summary>
        [JsonPropertyName("raw")]
        public string Raw { get; set; }
    }
}
