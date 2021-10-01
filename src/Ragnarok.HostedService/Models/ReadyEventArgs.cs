using Ragnarok.AgentApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ragnarok.HostedService.Models
{
    public class ReadyEventArgs : EventArgs
    {
        public IEnumerable<TunnelDetail> Tunnels { get; init; }
    }
}
