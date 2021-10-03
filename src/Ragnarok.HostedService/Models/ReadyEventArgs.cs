using Ragnarok.AgentApi.Models;
using System;
using System.Collections.Generic;

namespace Ragnarok.HostedService.Models
{
    /// <summary>
    /// Holds info about auto-created tunnels
    /// </summary>
    public class ReadyEventArgs : EventArgs
    {
        /// <summary>
        /// Tunnels created on application startup
        /// </summary>
        public IEnumerable<TunnelDetail> Tunnels { get; init; }
    }
}
