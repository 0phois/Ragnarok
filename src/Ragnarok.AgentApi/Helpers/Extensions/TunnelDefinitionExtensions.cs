using Ragnarok.AgentApi.Models;
using System.Collections.Generic;

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
        public static TunnelDefinition WithName(this TunnelDefinition tunnel, string name)
        {
            tunnel.Name = name.ToLower();
            return tunnel;
        }

        /// <summary>
        /// Tunnel protocol name, one of http, tcp, tls
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="proto"></param>
        /// <returns></returns>
        public static TunnelDefinition WithProtocol(this TunnelDefinition tunnel, TunnelProtocol proto)
        {
            tunnel.Protocol = proto;
            return tunnel;
        }

        /// <summary>
        /// The version of the proxy protocol to use with the tunnel
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="proxyProtocol"></param>
        /// <returns></returns>
        public static TunnelDefinition WithProtocolProxy(this TunnelDefinition tunnel, string proxyProtocol)
        {
            tunnel.ProxyProtocol = proxyProtocol;
            return tunnel;
        }

        /// <summary>
        /// Forward traffic to this local port number
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static TunnelDefinition WithPort(this TunnelDefinition tunnel, int port)
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
        public static TunnelDefinition WithAddress(this TunnelDefinition tunnel, string addr)
        {
            tunnel.Address = addr;
            return tunnel;
        }

        /// <summary>
        /// Rejects connections that <strong>do not match</strong> the given CIDRs
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="cidrs"></param>
        /// <returns></returns>
        public static TunnelDefinition AllowCIDRs(this TunnelDefinition tunnel, params string[] cidrs)
        {
            tunnel.IpRestriction ??= new();

            tunnel.IpRestriction.WhitelistIPs = cidrs;
            return tunnel;
        }

        /// <summary>
        /// Rejects connections that match the given CIDRs and <strong>allows all other</strong> CIDRs
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="cidrs"></param>
        /// <returns></returns>
        public static TunnelDefinition DenyCIDRs(this TunnelDefinition tunnel, params string[] cidrs)
        {
            tunnel.IpRestriction ??= new();

            tunnel.IpRestriction.BlacklistIPs = cidrs;
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
        public static TunnelDefinition WithAuthCredentials(this TunnelDefinition tunnel, string username, string password)
        {
            var auth = new AuthenticationCredentials() { Username = username, Password = password };
            tunnel.Auth = [auth];

            return tunnel;
        }

        /// <summary>
        /// Allow only OAuth2 users with these email domains
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="domains"></param>
        /// <returns></returns>
        public static TunnelDefinition AllowDomains(this TunnelDefinition tunnel, params string[] domains)
        {
            tunnel.OAuth ??= new();

            tunnel.OAuth.AllowDomains = domains;
            return tunnel;
        }

        /// <summary>
        /// Allow only OAuth users with these emails 
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="emails"></param>
        /// <returns></returns>
        public static TunnelDefinition AllowEmails(this TunnelDefinition tunnel, params string[] emails)
        {
            tunnel.OAuth ??= new();

            tunnel.OAuth.AllowEmails = emails;
            return tunnel;
        }

        /// <summary>
        /// Request these OAuth2 scopes when a user authenticates
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public static TunnelDefinition WithScopes(this TunnelDefinition tunnel, params string[] scopes)
        {
            tunnel.OAuth ??= new();

            tunnel.OAuth.Scopes = scopes;
            return tunnel;
        }

        /// <summary>
        /// Enforce authentication OAuth2 provider on the endpoint
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static TunnelDefinition WithOauthProvider(this TunnelDefinition tunnel, string provider)
        {
            tunnel.OAuth ??= new();

            tunnel.Scheme = Scheme.https;
            tunnel.OAuth.Provider = provider;
            return tunnel;
        }

        /// <summary>
        /// Reject requests when 5XX responses exceed this ratio
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        public static TunnelDefinition SetCircuitBreaker(this TunnelDefinition tunnel, double ratio)
        {
            tunnel.CircuitBreakerRatio = ratio;
            return tunnel;
        }

        /// <summary>
        /// Reject requests when 5XX responses exceed this ratio
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public static TunnelDefinition WithCompression(this TunnelDefinition tunnel, bool enabled = true)
        {
            tunnel.Compression = enabled;
            return tunnel;
        }

        /// <summary>
        /// Rewrite the HTTP Host header to this value, or <i><b>preserve</b></i> to leave it unchanged
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="hostHeader"></param>
        /// <returns></returns>
        public static TunnelDefinition WithHostHeader(this TunnelDefinition tunnel, string hostHeader)
        {
            tunnel.HostHeader = hostHeader;
            return tunnel;
        }

        /// <summary>
        /// Bind an HTTPS or HTTP endpoint or both
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public static TunnelDefinition WithScheme(this TunnelDefinition tunnel, Scheme scheme)
        {
            if (tunnel.OAuth?.Provider != null) { scheme = Scheme.https; }

            tunnel.Scheme = scheme;
            return tunnel;
        }

        /// <summary>
        /// Subdomain name to request
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="subdomain"></param>
        /// <returns></returns>
        public static TunnelDefinition WithSubdomain(this TunnelDefinition tunnel, string subdomain)
        {
            tunnel.Subdomain = subdomain.ToLower();
            return tunnel;
        }

        /// <summary>
        /// Domain/Hostname to request (requires reserved name and DNS CNAME)
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static TunnelDefinition WithDomain(this TunnelDefinition tunnel, string hostName)
        {
            tunnel.Domain = hostName;
            return tunnel;
        }

        /// <summary>
        /// PEM TLS certificate at this path to terminate TLS traffic before forwarding locally 
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="crt"></param>
        /// <returns></returns>
        public static TunnelDefinition WithCertificate(this TunnelDefinition tunnel, string crt)
        {
            tunnel.TerminateAt = TerminationPoint.edge;
            tunnel.Certificate = crt;
            return tunnel;
        }

        /// <summary>
        /// PEM TLS private key at this path to terminate TLS traffic before forwarding locally
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TunnelDefinition WithKey(this TunnelDefinition tunnel, string key)
        {
            tunnel.TerminateAt = TerminationPoint.edge;
            tunnel.Key = key;
            return tunnel;
        }

        /// <summary>
        /// PEM TLS certificate authority at this path will verify incoming TLS client connection certificates
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="tlsCasPath"></param>
        /// <returns></returns>
        public static TunnelDefinition WithClientCertificateAuthorities(this TunnelDefinition tunnel, string tlsCasPath)
        {
            tunnel.ClientCertificateAuthorities = tlsCasPath;
            return tunnel;
        }

        /// <summary>
        /// Bind the remote TCP port on the given address 
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="remoteAddress"></param>
        /// <returns></returns>
        public static TunnelDefinition WithRemoteAddress(this TunnelDefinition tunnel, string remoteAddress)
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
        public static TunnelDefinition WithMetaData(this TunnelDefinition tunnel, string metaData)
        {
            tunnel.MetaData = metaData;
            return tunnel;
        }

        /// <summary>
		/// The labels for this tunnel in the format name=value.
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="label"></param>
		/// <returns></returns>
        public static TunnelDefinition AddLabel(this TunnelDefinition tunnel, KeyValuePair<string, string> label)
        {
            tunnel.Protocol = TunnelProtocol.none;

            tunnel.Labels ??= [];

            tunnel.Labels.Add($"{label.Key}={label.Value}");

            return tunnel;
        }
    }
}
