using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// The User Agent Filter module enables you to block bots, crawlers, or certain browsers from accessing your web application
    /// </summary>
    public class UserAgentFilter
    {
        /// <summary>
        /// Allows User-Agents that match against these RE2 Regular Expressions <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("allow")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "allow", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string[] UserAgentFilterAllow { get; set; }

        /// <summary>
        /// Denies User-Agents that match against these RE2 Regular Expressions <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("deny")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "deny", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string[] UserAgentFilterDeny { get; set; }
    }
}
