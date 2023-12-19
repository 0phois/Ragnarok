using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// The IP Restrictions module allows or denies traffic based on the source IP of the connection that was initiated to your ngrok endpoints
    /// </summary>
    public class IpRestrictions
    {
        /// <summary>
        /// Rejects connections that <strong>do not match</strong> the given CIDRs <br/>
        /// </summary>
        [JsonPropertyName("allow_cidrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "allow_cidrs", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string[] WhitelistIPs { get; set; }

        /// <summary>
        /// Rejects connections that match the given CIDRs and <strong>allows all other</strong> CIDRs. <br/>
        /// </summary>
        [JsonPropertyName("deny_cidrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "deny_cidrs", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string[] BlacklistIPs { get; set; }
    }
}
