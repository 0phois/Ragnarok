using Ragnarok.AgentApi.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Reference: <see href="https://ngrok.com/docs#tunnel-definitions">https://ngrok.com/docs#tunnel-definitions</see>
    /// </summary>
    public class TunnelDefinition
    {
        /// <summary>
        /// Name of the tunnel
        /// </summary>
        [JsonPropertyName("name")]
        [YamlIgnore]
        public string Name { get; set; } = System.AppDomain.CurrentDomain.FriendlyName;

        /// <summary>
        /// Forward traffic to this local port number or network address <br/>
        /// <b>**Required**</b>
        /// </summary>
        [JsonPropertyName("addr")]
        [YamlMember(Alias = "addr")]
        public string Address { get; set; }

        /// <summary>
        /// Tunnel protocol
        /// </summary>
        [JsonIgnore]
        [YamlIgnore]
        public TunnelProtocol Protocol { get; set; } = TunnelProtocol.http;

        /// <summary>
        /// Tunnel protocol name, one of http, tcp, tls <br/>
        /// <b>**Required**</b>
        /// </summary>
        [JsonPropertyName("proto")]
        [YamlMember(Alias = "proto", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        public string Proto
        {
            get
            {
                if (Protocol == TunnelProtocol.none) return null;

                return Protocol.ToString();
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    Protocol = TunnelProtocol.none;
                else
                    Protocol = Enum.Parse<TunnelProtocol>(value);
            }
        }

        /// <summary>
        /// The version of PROXY protocol to use with this tunnel, empty if not using. <br/>
        /// <b>**Required**</b>
        /// </summary>
        [JsonPropertyName("proxy_proto")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "proxy_proto", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string ProxyProtocol { get; set; }

        /// <summary>
        /// Define rules which allow or deny connections from IPv4 or IPv6 CIDR blocks
        /// </summary>
        [JsonPropertyName("ip_restriction")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "ip_restriction", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public IpRestrictions IpRestriction { get; set; }

        //https://ngrok.com/docs/agent/config/#http-configuration
        #region HTTP Configuration
        /// <summary>
        /// HTTP basic authentication credentials to enforce on tunneled requests <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("basic_auth")]
        [JsonConverter(typeof(CredentialsConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "basic_auth", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public AuthenticationCredentials[] Auth { get; set; }

        /// <summary>
        /// Reject requests when 5XX responses exceed this ratio <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("circuit_breaker")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [YamlMember(Alias = "circuit_breaker", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        public double CircuitBreakerRatio { get; set; }

        /// <summary>
        /// Gzip compress HTTP responses from your web service <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("compression")]
        [YamlMember(Alias = "compression", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        public bool? Compression { get; set; }

        /// <summary>
        /// Rewrite the HTTP Host header to this value, or <i><b>preserve</b></i> to leave it unchanged <br/>
        /// <b><see cref="TunnelProtocol.http"/></b> <br/>
        /// </summary>
        [JsonPropertyName("host_header")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "host_header", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string HostHeader { get; set; }

        /// <summary>
        /// The domain to request. If using a custom domain, this requires registering in the 
        /// <a href="https://dashboard.ngrok.com/cloud-edge/domains">ngrok dashboard</a> and setting a DNS CNAME value. <br/>
        /// <b><see cref="TunnelProtocol.http"/> or <see cref="TunnelProtocol.tls"/></b> <br/>
        /// <i>Requires Paid Plan</i>
        /// </summary>
        [JsonPropertyName("domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "domain", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Domain { get; set; }

        /// <summary>
        /// Subdomain name to request. If unspecified, ngrok provides a unique subdomain based on your account type.
        /// <b><see cref="TunnelProtocol.http"/> or <see cref="TunnelProtocol.tls"/></b> <br/>
        /// <i>Requires Paid Plan</i>
        /// </summary>
        [JsonPropertyName("subdomain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "subdomain", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Subdomain { get; set; }

        /// <summary>
        /// Enable HTTP request inspection <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("inspect")]
        [YamlMember(Alias = "inspect", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        public bool? Inspect { get; set; }

        /// <summary>
        /// The path to the TLS certificate authority to verify client certs in mutual TLS <br/>
        /// <b><see cref="TunnelProtocol.http"/> or <see cref="TunnelProtocol.tls"/></b>
        /// </summary>
        [JsonPropertyName("mutual_tls_cas")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "mutual_tls_cas", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string ClientCertificateAuthorities { get; set; }

        /// <summary>
        /// Enforces a browser-based OAuth flow in front of your HTTP endpoints to an identity provider
        /// </summary>
        [JsonPropertyName("oauth")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "oauth", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public OAuth OAuth { get; set; }

        /// <summary>
        /// Add and remove headers from HTTP requests before they are sent to your upstream service
        /// </summary>
        [JsonPropertyName("request_header")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "request_header", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public RequestHeader RequestHeader { get; set; }

        /// <summary>
        /// Add and remove headers from the HTTP response before it is returned to the client
        /// </summary>
        [JsonPropertyName("response_header")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "response_header", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public ResponseHeader ResponseHeader { get; set; }

        /// <summary>
        /// Bind an HTTPS or HTTP endpoint or both <br/>
        /// <b><see cref="TunnelProtocol.http"/></b> <br/>
        /// </summary>
        [JsonIgnore]
        [YamlIgnore]
        public Scheme Scheme { get; set; } = Scheme.none;

        /// <summary>
        /// Bind an HTTPS or HTTP endpoint or both <br/>
        /// <b><see cref="TunnelProtocol.http"/></b> <br/>
        /// </summary>
        [JsonPropertyName("schemes")]
        [YamlMember(Alias = "schemes", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string[] Schemes
        {
            get
            {
                if (Scheme == Scheme.none) return null;

                return Scheme == Scheme.both ? ["http", "https"] : [Scheme.ToString().ToLower()];
            }
            private set
            {
                Scheme = value.Length == 2 ? Scheme.both : Enum.Parse<Scheme>(value[0]);
            }
        }

        /// <summary>
        /// Enables you to block bots, crawlers, or certain browsers 
        /// </summary>
        [JsonPropertyName("user_agent_filter")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "user_agent_filter", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public UserAgentFilter UserAgentFilter { get; set; }

        /// <summary>
        /// Authenticates that webhook requests sent to your HTTP endpoints are originated by your webhook provider 
        /// </summary>
        [JsonPropertyName("verify_webhook")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "verify_webhook", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public VerifyWebhook VerifyWebhook { get; set; }

        /// <summary>
        /// Convert ingress websocket connections to TCP upstream <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("websocket_tcp_converter")]
        [YamlMember(Alias = "websocket_tcp_converter", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        public bool? WebsocketTCPconverter { get; set; }
        #endregion

        //https://ngrok.com/docs/agent/config/#tcp-configuration
        #region TCP Configuration

        /// <summary>
        /// Bind the remote TCP port on the given address <br/>
        /// <b><see cref="TunnelProtocol.tcp"/></b>
        /// </summary>
        [JsonPropertyName("remote_addr")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "remote_addr", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string RemoteAddress { get; set; }

        #endregion

        //https://ngrok.com/docs/agent/config/#tls-configuration
        #region TLS Configuration

        /// <summary>
        /// PEM TLS certificate at this path to terminate TLS traffic before forwarding locally <br/>
        /// <b><see cref="TunnelProtocol.tls"/></b>
        /// </summary>
        [JsonPropertyName("crt")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "crt", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Certificate { get; set; }

        /// <summary>
        /// PEM TLS private key at this path to terminate TLS traffic before forwarding locally <br/>
        /// <b><see cref="TunnelProtocol.tls"/></b>
        /// </summary>
        [JsonPropertyName("key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "key", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Key { get; set; }

        /// <summary>
        /// Terminate at ngrok edge, agent, or upstream. Defaults to upstream unless --crt or --key are present, in which case edge is the default. <br/>
        /// <b><see cref="TunnelProtocol.tls"/></b>
        /// </summary>
        [JsonPropertyName("terminate_at")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "terminate_at", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public TerminationPoint? TerminateAt { get; set; }

        #endregion

        //https://ngrok.com/docs/agent/config/#labeled-tunnel-configuration
        #region Labeled Tunnel Configuration
        /// <summary>
        /// The labels for this tunnel in the format name=value. <br/>
        /// <b><see cref="TunnelProtocol.http"/></b>
        /// </summary>
        [JsonPropertyName("labels")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [YamlMember(Alias = "labels", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> Labels { get; set; }
        #endregion

        /// <summary>
        /// Arbitrary user-defined metadata that will appear in the ngrok service API when listing tunnels
        /// </summary>
		[JsonPropertyName("metadata")]
        [YamlMember(Alias = "metadata", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string MetaData { get; set; }
    }
}
