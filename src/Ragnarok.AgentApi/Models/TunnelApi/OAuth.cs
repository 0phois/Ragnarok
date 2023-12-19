using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// The OAuth module enforces a browser-based OAuth flow in front of your HTTP endpoints to an identity provider.
    /// </summary>
    public class OAuth
    {
        /// <summary>
        /// Allow only OAuth2 users with these email domains <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("allow_domains")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "allow_domains", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string[] AllowDomains { get; set; }

        /// <summary>
        /// Allow only OAuth users with these emails <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("allow_emails")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "allow_emails", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string[] AllowEmails { get; set; }

        /// <summary>
        /// Request these OAuth2 scopes when a user authenticates <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("oauth_scopes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "oauth_scopes", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string[] Scopes { get; set; }

        /// <summary>
        /// Enforce authentication OAuth2 provider on the endpoint, e.g. 'google'. <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("provider")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "provider", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Provider { get; set; }

        /// <summary>
        /// Client ID of the provider
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("client_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "client_id", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string ClientId { get; set; }

        /// <summary>
        /// Client secret for the provider
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("client_secret")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "client_secret", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string ClientSecret { get; set; }
    }
}
