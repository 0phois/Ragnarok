using Ragnarok.AgentApi.Helpers;
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
		[YamlIgnore]
		[JsonPropertyName("name")]
		public string Name { get; set; } = System.AppDomain.CurrentDomain.FriendlyName;

		/// <summary>
		/// Tunnel protocol name, one of http, tcp, tls <br/>
		/// <b>**Required**</b>
		/// </summary>
		[YamlMember(Alias = "proto")]
		[JsonPropertyName("proto")]
		public TunnelProtocol Protocol { get; set; } = TunnelProtocol.HTTP;

		/// <summary>
		/// Forward traffic to this local port number or network address <br/>
		/// <b>**Required**</b>
		/// </summary>
		[YamlMember(Alias = "addr")]
		[JsonPropertyName("addr")]
		public string Address { get; set; } = "80";

		/// <summary>
		/// Enable HTTP request inspection <br/>
		/// <b><see cref="TunnelProtocol.HTTP"/></b>
		/// </summary>
		[YamlMember(Alias = "inspect")]
		[JsonPropertyName("inspect")]
		public bool Inspect { get; set; }

		/// <summary>
		/// HTTP basic authentication credentials to enforce on tunneled requests <br/>
		/// <b><see cref="TunnelProtocol.HTTP"/></b>
		/// </summary>
		[JsonConverter(typeof(CredentialsConverter))]
		[YamlMember(Alias = "auth")]
		[JsonPropertyName("auth")]
		public AuthenticationCredentials Auth { get; set; }

		/// <summary>
		/// Rewrite the HTTP Host header to this value, or <i><b>preserve</b></i> to leave it unchanged <br/>
		/// <b><see cref="TunnelProtocol.HTTP"/></b> <br/>
		/// </summary>
		[YamlMember(Alias = "host_header")]
		[JsonPropertyName("host_header")]
		public string HostHeader { get; set; }

		/// <summary>
		/// Bind an HTTPS or HTTP endpoint or both <br/>
		/// <b><see cref="TunnelProtocol.HTTP"/></b> <br/>
		/// </summary>
		[YamlMember(Alias = "bind_tls")]
		[JsonPropertyName("bind_tls")]
		public BindTLS BindTLS { get; set; }

		/// <summary>
		/// Subdomain name to request <br/>
		/// <b><see cref="TunnelProtocol.HTTP"/> or <see cref="TunnelProtocol.TLS"/></b> <br/>
		/// <i>Requires Paid Plan</i>
		/// </summary>
		[YamlMember(Alias = "subdomain")]
		[JsonPropertyName("subdomain")]
		public string Subdomain { get; set; }

		/// <summary>
		/// Hostname to request (requires reserved name and DNS CNAME) <br/>
		/// <b><see cref="TunnelProtocol.HTTP"/> or <see cref="TunnelProtocol.TLS"/></b>
		/// </summary>
		[YamlMember(Alias = "hostname")]
		[JsonPropertyName("hostname")]
		public string HostName { get; set; }

		/// <summary>
		/// PEM TLS certificate at this path to terminate TLS traffic before forwarding locally <br/>
		/// <b><see cref="TunnelProtocol.TLS"/></b>
		/// </summary>
		[YamlMember(Alias = "crt")]
		[JsonPropertyName("crt")]
		public string Certificate { get; set; }

		/// <summary>
		/// PEM TLS private key at this path to terminate TLS traffic before forwarding locally <br/>
		/// <b><see cref="TunnelProtocol.TLS"/></b>
		/// </summary>
		[YamlMember(Alias = "key")]
		[JsonPropertyName("key")]
		public string Key { get; set; }

		/// <summary>
		/// PEM TLS certificate authority at this path will verify incoming TLS client connection certificates <br/>
		/// <b><see cref="TunnelProtocol.TLS"/></b>
		/// </summary>
		[YamlMember(Alias = "client_cas")]
		[JsonPropertyName("client_cas")]
		public string ClientCertificateAuthorities { get; set; }

		/// <summary>
		/// Bind the remote TCP port on the given address <br/>
		/// <b><see cref="TunnelProtocol.TCP"/></b>
		/// </summary>
		[YamlMember(Alias = "remote_addr")]
		[JsonPropertyName("remote_addr")]
		public string RemoteAddress { get; set; }

		/// <summary>
		/// Arbitrary user-defined metadata that will appear in the ngrok service API when listing tunnels
		/// </summary>
		[YamlMember(Alias = "metadata")]
		[JsonPropertyName("metadata")]
		public string MetaData { get; set; }
	}
}
