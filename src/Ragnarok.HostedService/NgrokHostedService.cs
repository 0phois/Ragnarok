using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ragnarok.AgentApi;
using Ragnarok.AgentApi.Extensions;
using Ragnarok.AgentApi.Models;
using Ragnarok.HostedService.Contracts;
using Ragnarok.HostedService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ragnarok.HostedService
{
    /// <summary>
    /// Manages the ngrok process
    /// </summary>
    public class NgrokHostedService : INgrokHostedService, IDisposable
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<ReadyEventArgs> Ready;

        /// <summary>
        /// Raises the <see cref="Ready"/> event
        /// </summary>
        /// <param name="tunnels">The created tunnels</param>
        protected virtual void OnReady(IEnumerable<TunnelDetail> tunnels) => Ready?.Invoke(this, new ReadyEventArgs() { Tunnels = tunnels });

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public RagnarokClient RagnarokClient { get; }

        private readonly IServer _server;
        private readonly string _authToken;
        private readonly ILogger<NgrokHostedService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;

        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <param name="logger"><see cref="ILogger"/> instance for logging <see cref="NgrokHostedService"/></param>
        /// <param name="applicationLifetime">Allows consumers to be notified of application lifetime events</param>
        public NgrokHostedService(RagnarokClient client,
                                  IServer server,
                                  ILogger<NgrokHostedService> logger,
                                  IHostApplicationLifetime applicationLifetime) : this(client, server, null, logger, applicationLifetime) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <param name="token">ngrok authtoken</param>
        /// <param name="logger"><see cref="ILogger"/> instance for logging <see cref="NgrokHostedService"/></param>
        /// <param name="applicationLifetime">Allows consumers to be notified of application lifetime events</param>
        public NgrokHostedService(RagnarokClient client, IServer server, AuthToken token, 
                                  ILogger<NgrokHostedService> logger, IHostApplicationLifetime applicationLifetime)
        {
            RagnarokClient = client;

            _logger = logger;
            _server = server;
            _authToken = token?.RawValue;
            _appLifetime = applicationLifetime;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(async () => 
            {
                var tunnels = new List<TunnelDetail>();
                var addresses = GetApplicationUrls();

                if (!string.IsNullOrWhiteSpace(_authToken)) await RagnarokClient.RegisterAuthTokenAsync(_authToken);

                foreach (var address in addresses)
                {
                    var bind = address.StartsWith("http://") ? BindTLS.Both : BindTLS.True;
                    var tunnel = await RagnarokClient.ConnectAsync(new TunnelDefinition().Address(address).BindTLS(bind), 
                                                                   cancellationToken: cancellationToken);

                    if (tunnel != null) tunnels.Add(tunnel);

                    _logger?.LogInformation("Started tunnel {name}: {url} -> {localAddress}", tunnel.Name, tunnel.PublicURL, address);
                }

                OnReady(tunnels);
            });

            return Task.CompletedTask;
        }

        private IEnumerable<string> GetApplicationUrls()
        {
            var addresses = _server.Features.Get<IServerAddressesFeature>().Addresses;
            var urls = addresses.Select(x => x = x.Replace("*", "localhost", StringComparison.InvariantCulture)).DefaultIfEmpty("80");

            return urls.Distinct();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
           await RagnarokClient.DisconnectAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            RagnarokClient.StopNgrokProcess();
            RagnarokClient.Dispose();
        }
    }
}
