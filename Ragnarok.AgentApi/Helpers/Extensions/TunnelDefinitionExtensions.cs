using Ragnarok.AgentApi.Models;

namespace Ragnarok.AgentApi.Extensions
{
	public static class TunnelDefinitionExtensions
	{
		public static TunnelDefinition Name(this TunnelDefinition tunnel, string name)
		{
			tunnel.Name = name;
			return tunnel;
		}

		public static TunnelDefinition Protocol(this TunnelDefinition tunnel, TunnelProtocol proto)
		{
			tunnel.Protocol = proto;
			return tunnel;
		}

		public static TunnelDefinition Port(this TunnelDefinition tunnel, int port)
		{
			tunnel.Address = port.ToString();
			return tunnel;
		}

		public static TunnelDefinition Address(this TunnelDefinition tunnel, string addr) 
		{
			tunnel.Address = addr;
			return tunnel;
		}

		public static TunnelDefinition Inspect(this TunnelDefinition tunnel, bool inspect)
		{
			tunnel.Inspect = inspect;
			return tunnel;
		}

		public static TunnelDefinition Auth(this TunnelDefinition tunnel, string username, string password)
		{
			var auth = new AuthenticationCredentials() { Username = username, Password = password };
			tunnel.Auth = auth;

			return tunnel;
		}
		
		public static TunnelDefinition HostHeader(this TunnelDefinition tunnel, string hostHeader)
		{
			tunnel.HostHeader = hostHeader;
			return tunnel;
		}
		
		public static TunnelDefinition BindTLS(this TunnelDefinition tunnel, BindTLS bindTls)
		{
			tunnel.BindTLS = bindTls;
			return tunnel;
		}
		
		public static TunnelDefinition Subdomain(this TunnelDefinition tunnel, string subdomain)
		{
			tunnel.Subdomain = subdomain;
			return tunnel;
		}

		public static TunnelDefinition HostName(this TunnelDefinition tunnel, string hostName)
		{
			tunnel.HostName = hostName;
			return tunnel;
		}
		
		public static TunnelDefinition Certificate(this TunnelDefinition tunnel, string crt)
		{
			tunnel.Certificate = crt;
			return tunnel;
		}
		
		public static TunnelDefinition Key(this TunnelDefinition tunnel, string key)
		{
			tunnel.Key = key;
			return tunnel;
		}
		
		public static TunnelDefinition ClientCertificateAuthorities(this TunnelDefinition tunnel, string clientCas)
		{
			tunnel.ClientCertificateAuthorities = clientCas;
			return tunnel;
		}

		public static TunnelDefinition RemoteAddress(this TunnelDefinition tunnel, string remoteAddress)
		{
			tunnel.RemoteAddress = remoteAddress;
			return tunnel;
		}

		public static TunnelDefinition MetaData(this TunnelDefinition tunnel, string metaData)
		{
			tunnel.MetaData = metaData;
			return tunnel;
		}
	}
}
