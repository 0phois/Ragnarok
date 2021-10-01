using System;

namespace Ragnarok.AgentApi.Models
{
    public class TerminateEventArgs : EventArgs
    {
        public int ExitCode { get; init; }
        public int ProcessId { get; init; }
    }
}
