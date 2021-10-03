using Microsoft.Extensions.Hosting;
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
        event EventHandler Ready;
    }
}
