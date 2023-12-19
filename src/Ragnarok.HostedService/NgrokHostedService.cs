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
        private readonly ILogger<NgrokHostedService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly TunnelDefinition _tunnelDefinition;

        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <param name="logger"><see cref="ILogger"/> instance for logging <see cref="NgrokHostedService"/></param>
        /// <param name="applicationLifetime">Allows consumers to be notified of application lifetime events</param>
        public NgrokHostedService(RagnarokClient client, IServer server, ILogger<NgrokHostedService> logger,
                                  IHostApplicationLifetime applicationLifetime) : this(client, server, null, logger, applicationLifetime) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <param name="tunnelDefinition">Configuration for a tunnel to start</param>
        /// <param name="logger"><see cref="ILogger"/> instance for logging <see cref="NgrokHostedService"/></param>
        /// <param name="applicationLifetime">Allows consumers to be notified of application lifetime events</param>
        public NgrokHostedService(RagnarokClient client, IServer server, Action<TunnelDefinition> tunnelDefinition,
                                  ILogger<NgrokHostedService> logger, IHostApplicationLifetime applicationLifetime)
        {
            RagnarokClient = client;

            _logger = logger;
            _server = server;
            _tunnelDefinition = new();
            _appLifetime = applicationLifetime;

            tunnelDefinition?.Invoke(_tunnelDefinition);
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
                var tunnels = _tunnelDefinition is null ? RagnarokClient.Config.Tunnels?.Values.ToArray() ?? [] : [_tunnelDefinition];
                var tunnelDetails = new List<TunnelDetail>();
                var addresses = GetApplicationUrls();

                if (tunnels.Length > 0)
                {
                    await CreateConfiguredTunnels(tunnels, addresses, tunnelDetails, cancellationToken);
                }
                else
                {
                    await CreateDefaultTunnels(addresses, tunnelDetails, cancellationToken);
                }

                OnReady(tunnelDetails);
            });

            return Task.CompletedTask;
        }

        private async Task CreateConfiguredTunnels(IEnumerable<TunnelDefinition> tunnels, IEnumerable<string> addresses,
                                                   List<TunnelDetail> tunnelDetails, CancellationToken cancellationToken)
        {
            foreach (var tunnel in tunnels)
            {
                string[] targetAddress = string.IsNullOrEmpty(tunnel.Address) ? addresses.ToArray() : [tunnel.Address];

                foreach (var address in targetAddress)
                {
                    TunnelDetail tunnelDetail = await RagnarokClient.ConnectAsync(tunnel.WithAddress(address), cancellationToken: cancellationToken)
                                                                    .ConfigureAwait(false);

                    if (tunnelDetail is not null)
                    {
                        tunnelDetails.Add(tunnelDetail);
                        _logger?.LogInformation("Started tunnel {name}: {url} -> {localAddress}", tunnelDetail.Name, tunnelDetail.PublicURL, address);
                    }
                }
            }
        }

        private async Task CreateDefaultTunnels(IEnumerable<string> addresses, List<TunnelDetail> tunnelDetails, CancellationToken cancellationToken)
        {
            foreach (var address in addresses)
            {
                var uri = new Uri(address);
                var scheme = address.StartsWith("http://") ? Scheme.both : Scheme.https;
                var tunnelDetail = await RagnarokClient.ConnectAsync(options =>
                {
                    options.WithAddress(address);
                    options.WithHostHeader(uri.Authority);
                    options.WithScheme(scheme);
                },
                cancellationToken: cancellationToken).ConfigureAwait(false);

                if (tunnelDetail is not null)
                {
                    tunnelDetails.Add(tunnelDetail);
                    _logger?.LogInformation("Started tunnel {name}: {url} -> {localAddress}", tunnelDetail.Name, tunnelDetail.PublicURL, address);
                }
            }
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
            RagnarokClient.StopNgrokProcess();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            RagnarokClient.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
