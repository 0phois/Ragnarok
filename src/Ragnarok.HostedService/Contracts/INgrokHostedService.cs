using Microsoft.Extensions.Hosting;
using Ragnarok.AgentApi;
using Ragnarok.HostedService.Models;
using System;

namespace Ragnarok.HostedService.Contracts
{
#pragma warning disable CS1591
    public interface INgrokHostedService : IHostedService
#pragma warning restore CS1591
    {
        /// <summary>
        /// Raised when all tunnels have been created
        /// </summary>
        event EventHandler<ReadyEventArgs> Ready;

        /// <summary>
        /// An instance of <see cref="RagnarokClient"/> that provides access to the ngrok client process and the ngrok Agent Api
        /// </summary>
        public RagnarokClient RagnarokClient { get; }
    }
}
