using System;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Holds info about a created tunnel
    /// </summary>
    public class TunnelCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Address of the created tunnel
        /// </summary>
        public string TunnelAddress { get; init; }
        /// <summary>
        /// Name of the creted tunnel
        /// </summary>
        public string TunnelName { get; init; }
        /// <summary>
        /// Public url of the created tunnel
        /// </summary>
        public string TunnelUrl { get; init; }
    }
}
