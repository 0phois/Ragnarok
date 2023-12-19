using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// This module adds and removes headers from the HTTP response before it is returned to the client
    /// </summary>
    public class ResponseHeader
    {
        /// <summary>
        /// The headers to add to the response in the key:value format. <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("add")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "add", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string[] Add { get; set; }

        /// <summary>
        /// The header keys to remove from the response. <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("remove")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "remove", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string[] Remove { get; set; }
    }
}
