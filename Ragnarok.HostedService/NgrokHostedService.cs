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
    public class NgrokHostedService : INgrokHostedService, IDisposable
    {
        public event EventHandler Ready;
        protected virtual void OnReady() => Ready?.Invoke(this, EventArgs.Empty);

        public RagnarokClient RagnarokClient { get; }

        private readonly IServer _server;
        private readonly string _authToken;
        private readonly ILogger<NgrokHostedService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;

        public NgrokHostedService(RagnarokClient client,
                                  IServer server,
                                  ILogger<NgrokHostedService> logger,
                                  IHostApplicationLifetime appLifetime) : this(client, server, null, logger, appLifetime) { }

        public NgrokHostedService(RagnarokClient client, IServer server, AuthToken token, 
                                  ILogger<NgrokHostedService> logger, IHostApplicationLifetime applicationLifetime)
        {
            RagnarokClient = client;

            _logger = logger;
            _server = server;
            _authToken = token?.RawValue;
            _appLifetime = applicationLifetime;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(async () => 
            {
                var addresses = GetApplicationUrls();

                foreach (var address in addresses)
                {
                    var bind = address.StartsWith("http://") ? BindTLS.Both : BindTLS.True;
                    var tunnel = await RagnarokClient.ConnectAsync(new TunnelDefinition().Address(address).BindTLS(bind), _authToken, cancellationToken);

                    _logger?.LogInformation("Started tunnel {name}: {url} -> {localAddress}", tunnel.Name, tunnel.PublicURL, address);
                }

                OnReady();
            });

            return Task.CompletedTask;
        }

        private IEnumerable<string> GetApplicationUrls()
        {
            var addresses = _server.Features.Get<IServerAddressesFeature>().Addresses;
            var urls = addresses.Select(x => x = x.Replace("*", "localhost", StringComparison.InvariantCulture)).DefaultIfEmpty("80");

            return urls.Distinct();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
           await RagnarokClient.DisconnectAsync(cancellation: cancellationToken);
        }

        public void Dispose()
        {
            RagnarokClient.StopNgrokProcess();
            RagnarokClient.Dispose();
        }
    }
}
