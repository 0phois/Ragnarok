using System;

namespace Ragnarok.AgentApi.Models
{
    public class TunnelCreatedEventArgs : EventArgs
    {
        public string TunnelAddress { get; init; }
        public string TunnelName { get; init; }
        public string TunnelUrl { get; init; }
    }
}
