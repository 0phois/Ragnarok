using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Ragnarok.AgentApi.Models
{
	//https://ngrok.com/docs#config-examples
	public class NgrokConfiguration
    {
		/// <summary>
		/// Your authtoken from <see href="https://dashboard.ngrok.com/get-started/your-authtoken">dashboard.ngrok.com</see>
		/// </summary>
		/// <remarks>
		/// Specifies the authentication token used to authenticate this client when it connects to the ngrok.com service.<br/> 
		/// After you've created an ngrok.com account, your dashboard will display the authtoken assigned to your account.
		/// </remarks>
		[YamlMember(Alias = "authtoken")]
		public string AuthToken { get; set; }

		/// <summary>
		/// The region where the ngrok client will connect to host its tunnels
		/// </summary>
		/// <remarks>
		/// An ngrok client may only be connected a single region, 
		/// a single ngrok client cannot host tunnels in multiple regions simultaneously. <br/>
		/// The region used in the first tunnel will be used for all the following tunnels.
		/// Run multiple ngrok clients if you need to do this.
		/// </remarks>
		[YamlMember(Alias = "region")] 
		public NgrokRegion Region { get; set; }

		/// <summary>
		/// Enable of disable the console interface
		/// </summary>
		[YamlMember(Alias = "console_ui")] 
		public ConsoleOptions ConsoleUI { get; set; }

		/// <summary>
		/// The <see cref="ConsoleColor"/> of the console interface background
		/// </summary>
		[YamlMember(Alias = "console_ui_color")] 
		public ConsoleColor ConsoleColor { get; set; }

		/// <summary>
		/// URL of an HTTP proxy to use for establishing the tunnel connection
		/// </summary>
		[YamlMember(Alias = "http_proxy")] 
		public string HttpProxy { get; set; }

        /// <summary>
        /// Size in bytes of the upper limit on memory to allocate to save requests over HTTP tunnels for inspection and replay
        /// </summary>
        /// <remarks>
        /// <code> 0 - Use the default allocation limit [50MB]</code>
        /// <code>-1 - Disable inspection for all tunnels</code>
        /// </remarks>
        [YamlMember(Alias = "inspect_db_size")]
        public int InsepectDatabaseByteSize { get; set; }

		/// <summary>
		/// Logging level of detail
		/// </summary>
		[YamlMember(Alias = "log_level")] 
		public NgrokConfigLogLevel LogLevel { get; set; }

		/// <summary>
		/// Format of written log records
		/// </summary>
		[YamlMember(Alias = "log_format")]
		public NgrokLogFormat LogFormat { get; set; }

		/// <summary>
		/// Write logs to this target destination
		/// </summary>
		[YamlMember(Alias = "log")]
		public string Log { get; set; }

		/// <summary>
		/// User-supplied string that will be returned as part of the ngrok.com API response 
		/// to the List Online Tunnels resource for all tunnels started by this client.
		/// </summary>
		[YamlMember(Alias = "metadata")] 
		public string MetaData { get; set; }

		/// <summary>
		/// The root certificate authorities used to validate the TLS connection to the ngrok server
		/// </summary>
		[YamlMember(Alias = "root_cas")] 
		public string RootCertificateAuthorities { get; set; }

		/// <summary>
		/// URL of a SOCKS5 proxy to use for establishing a connection to the ngrok server
		/// </summary>
		[YamlMember(Alias = "socks5_proxy")] 
		public string Socks5Proxy { get; set; }

		/// <summary>
		/// Whether or not to automatically update 
		/// </summary>
		[YamlMember(Alias = "update")] 
		public bool AutoUpdateNgrok { get; set; }

		/// <summary>
		/// The update channel determines the stability of released builds to update to
		/// </summary>
		[YamlMember(Alias = "update_channel")] 
		public NgrokUpdateChannel UpdateChannel { get; set; }

		/// <summary>
		/// Network address to bind on for serving the local web interface and api
		/// </summary>
		[YamlMember(Alias = "web_addr")] 
		public string WebAddress { get; set; }

		/// <summary>
		/// A map of names to tunnel definitions
		/// </summary>
		[YamlMember(Alias = "tunnels")]
		public Dictionary<string, TunnelDefinition> Tunnels { get; set; }
	}
}