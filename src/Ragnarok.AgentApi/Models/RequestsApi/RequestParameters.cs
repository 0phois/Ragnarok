using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Query parameters for the List Captured Requests endpoint
    /// </summary>
    public class RequestParameters
    {
        /// <summary>
        /// Maximum number of requests to return
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Filter requests only for the given tunnel name
        /// </summary>
        [JsonPropertyName("tunnel_name")]
        public string TunnelName { get; set; }
    }
}
