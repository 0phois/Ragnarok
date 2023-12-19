using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Response object for the List Tunnels endpoint
    /// <see href="https://ngrok.com/docs#list-tunnels"/>
    /// </summary>
    public class TunnelList
    {
        /// <summary>
        /// Api endpoint
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        /// <summary>
        /// List of <see cref="TunnelDetail"/>
        /// </summary>
        [JsonPropertyName("tunnels")]
        public IEnumerable<TunnelDetail> Tunnels { get; set; }
    }
}
