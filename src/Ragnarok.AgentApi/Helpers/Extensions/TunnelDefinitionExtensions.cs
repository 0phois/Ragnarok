using Ragnarok.AgentApi.Models;

namespace Ragnarok.AgentApi.Extensions
{
	/// <summary>
	/// Fluent builder extensions for <see cref="TunnelDefinition"/>
	/// </summary>
	public static class TunnelDefinitionExtensions
	{
		/// <summary>
		/// Name of the tunnel
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static TunnelDefinition Name(this TunnelDefinition tunnel, string name)
		{
			tunnel.Name = name;
			return tunnel;
		}

		/// <summary>
		/// Tunnel protocol name, one of http, tcp, tls
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="proto"></param>
		/// <returns></returns>
		public static TunnelDefinition Protocol(this TunnelDefinition tunnel, TunnelProtocol proto)
		{
			tunnel.Protocol = proto;
			return tunnel;
		}

		/// <summary>
		/// Forward traffic to this local port number
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		public static TunnelDefinition Port(this TunnelDefinition tunnel, int port)
		{
			tunnel.Address = port.ToString();
			return tunnel;
		}

		/// <summary>
		/// Forward traffic to this local network address
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="addr"></param>
		/// <returns></returns>
		public static TunnelDefinition Address(this TunnelDefinition tunnel, string addr) 
		{
			tunnel.Address = addr;
			return tunnel;
		}

		/// <summary>
		/// Enable HTTP request inspection 
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="inspect"></param>
		/// <returns></returns>
		public static TunnelDefinition Inspect(this TunnelDefinition tunnel, bool inspect)
		{
			tunnel.Inspect = inspect;
			return tunnel;
		}

		/// <summary>
		/// HTTP basic authentication credentials to enforce on tunneled requests
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static TunnelDefinition Auth(this TunnelDefinition tunnel, string username, string password)
		{
			var auth = new AuthenticationCredentials() { Username = username, Password = password };
			tunnel.Auth = auth;

			return tunnel;
		}

		/// <summary>
		/// Rewrite the HTTP Host header to this value, or <i><b>preserve</b></i> to leave it unchanged
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="hostHeader"></param>
		/// <returns></returns>
		public static TunnelDefinition HostHeader(this TunnelDefinition tunnel, string hostHeader)
		{
			tunnel.HostHeader = hostHeader;
			return tunnel;
		}

		/// <summary>
		/// Bind an HTTPS or HTTP endpoint or both
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="bindTls"></param>
		/// <returns></returns>
		public static TunnelDefinition BindTLS(this TunnelDefinition tunnel, BindTLS bindTls)
		{
			tunnel.BindTLS = bindTls;
			return tunnel;
		}

		/// <summary>
		/// Subdomain name to request
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="subdomain"></param>
		/// <returns></returns>
		public static TunnelDefinition Subdomain(this TunnelDefinition tunnel, string subdomain)
		{
			tunnel.Subdomain = subdomain;
			return tunnel;
		}

		/// <summary>
		/// Hostname to request (requires reserved name and DNS CNAME)
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="hostName"></param>
		/// <returns></returns>
		public static TunnelDefinition HostName(this TunnelDefinition tunnel, string hostName)
		{
			tunnel.HostName = hostName;
			return tunnel;
		}

		/// <summary>
		/// PEM TLS certificate at this path to terminate TLS traffic before forwarding locally 
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="crt"></param>
		/// <returns></returns>
		public static TunnelDefinition Certificate(this TunnelDefinition tunnel, string crt)
		{
			tunnel.Certificate = crt;
			return tunnel;
		}

		/// <summary>
		/// PEM TLS private key at this path to terminate TLS traffic before forwarding locally
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static TunnelDefinition Key(this TunnelDefinition tunnel, string key)
		{
			tunnel.Key = key;
			return tunnel;
		}

		/// <summary>
		/// PEM TLS certificate authority at this path will verify incoming TLS client connection certificates
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="clientCas"></param>
		/// <returns></returns>
		public static TunnelDefinition ClientCertificateAuthorities(this TunnelDefinition tunnel, string clientCas)
		{
			tunnel.ClientCertificateAuthorities = clientCas;
			return tunnel;
		}

		/// <summary>
		/// Bind the remote TCP port on the given address 
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="remoteAddress"></param>
		/// <returns></returns>
		public static TunnelDefinition RemoteAddress(this TunnelDefinition tunnel, string remoteAddress)
		{
			tunnel.RemoteAddress = remoteAddress;
			return tunnel;
		}

		/// <summary>
		/// Arbitrary user-defined metadata that will appear in the ngrok service API when listing tunnels
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="metaData"></param>
		/// <returns></returns>
		public static TunnelDefinition MetaData(this TunnelDefinition tunnel, string metaData)
		{
			tunnel.MetaData = metaData;
			return tunnel;
		}
	}
}
