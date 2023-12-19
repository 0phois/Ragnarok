using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// The Webhook Verification module authenticates that webhook requests sent to your HTTP endpoints are originated by your webhook provider and intended for you.
    /// </summary>
    public class VerifyWebhook
    {
        /// <summary>
        /// Verify webhooks are signed by this provider <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("provider")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "provider", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Provider { get; set; }

        /// <summary>
        /// The secret used by provider to sign webhooks, if there is one <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("secret")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "secret", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Secret { get; set; }
    }
}
