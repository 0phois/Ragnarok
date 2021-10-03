using System;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Holds info about the terminated ngrok process
    /// </summary>
    public class TerminateEventArgs : EventArgs
    {
        /// <summary>
        /// The code specified when the process terminated
        /// </summary>
        public int ExitCode { get; init; }
        /// <summary>
        /// Unique identifier for the process
        /// </summary>
        public int ProcessId { get; init; }
    }
}
