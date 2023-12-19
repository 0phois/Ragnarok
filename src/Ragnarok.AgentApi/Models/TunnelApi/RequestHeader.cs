using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// This module adds and removes headers from HTTP requests before they are sent to your upstream service. 
    /// </summary>
    public class RequestHeader
    {
        /// <summary>
        /// The headers to add to the request in the key:value format. <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("add")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "add", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string[] Add { get; set; }

        /// <summary>
        /// The header keys to remove from the request. <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("remove")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "remove", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string[] Remove { get; set; }
    }
}
