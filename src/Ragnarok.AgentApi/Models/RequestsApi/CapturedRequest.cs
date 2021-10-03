using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Response object for the Captured Request Detail endpoint
    /// </summary>
    public class CapturedRequest
    {
        /// <summary>
        /// Api endpoint
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        /// <summary>
        /// List of <see cref="RequestDetail"/>
        /// </summary>
        [JsonPropertyName("requests")]
        public List<RequestDetail> Requests { get; set; }
    }
}
