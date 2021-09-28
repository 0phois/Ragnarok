using Ragnarok.AgentApi.Models;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace Ragnarok.AgentApi
{
    public partial class RagnarokClient
    {
        /// <summary>
        /// Raised when the ngrok session is established
        /// </summary>
        public event EventHandler Connected;
        protected virtual void OnConnected() => Connected?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Raised when the ngrok session is closed <br/>
        /// <i>Does not fire when the process is terminated</i>
        /// </summary>
        public event EventHandler Disconnected;
        protected virtual void OnDisconnected() => Disconnected?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Raised when the ngrok process exits
        /// </summary>
        public event EventHandler<TerminateEventArgs> Terminated;
        protected virtual void OnTerminated(Process process) 
            => Terminated?.Invoke(this, new TerminateEventArgs() { ProcessId = process.Id, ExitCode = process.ExitCode});

        /// <summary>
        /// Raised when a tunnel is successfully started
        /// </summary>
        public event EventHandler TunnelCreated;
        protected virtual void OnTunnelCreated(TunnelCreatedEventArgs args) 
            => TunnelCreated?.Invoke(this, args);

        private void RedirectProcesOutput()
        {
            if (Config.Log.Equals("stdout", StringComparison.OrdinalIgnoreCase))
                Ngrok.Process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => 
                { 
                    if (!string.IsNullOrWhiteSpace(e.Data)) StatusChanged(e.Data); 
                };

            if (Config.Log.Equals("stderr", StringComparison.OrdinalIgnoreCase))
                Ngrok.Process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data)) StatusChanged(e.Data);
                };
        }

        private void StatusChanged(string message)
        {
            if (message.Contains("client session established"))
                OnConnected();
            else if (message.Contains("session closed, starting reconnect loop"))
                OnDisconnected();
            else if (message.Contains(@"""addr"":")) 
            {
                var tunnelInfo = JsonSerializer.Deserialize<NgrokCondensedTunnelLog>(message);
                if (tunnelInfo.Message.Equals("started tunnel", StringComparison.OrdinalIgnoreCase))
                    OnTunnelCreated(new TunnelCreatedEventArgs() 
                    { 
                        TunnelAddress = tunnelInfo.Address,
                        TunnelName = tunnelInfo.Name, 
                        TunnelUrl = tunnelInfo.Url 
                    });
            } ;
        }
    }
}