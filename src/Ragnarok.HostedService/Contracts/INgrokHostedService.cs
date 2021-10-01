using Microsoft.Extensions.Hosting;
using System;

namespace Ragnarok.HostedService.Contracts
{
    public interface INgrokHostedService : IHostedService
    {
        event EventHandler Ready;
    }
}
