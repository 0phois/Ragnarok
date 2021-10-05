using Ragnarok.AgentApi.Exceptions;
using Ragnarok.AgentApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ragnarok.AgentApi.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="RagnarokClient"/>
    /// </summary>
    public static class RagnorokClientExtensions
    {
        /// <summary>
        /// Start ngrok and open a tunnel
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="authToken">Authorization token to register in ngrok.yml</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="TunnelDetail"/></returns>
        /// <remarks>
        /// <see cref="RagnarokClient.InitializeAsync"/> will be called if not previously executed
        /// </remarks>
        public static async Task<TunnelDetail> ConnectAsync(this RagnarokClient client, string authToken = null, CancellationToken cancellationToken = default)
            => await ConnectAsync(client, new TunnelDefinition(), authToken, cancellationToken);

        /// <summary>
        /// Start ngrok and open a tunnel
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="port">Local port number to forward traffic</param>
        /// <param name="authToken">Authorization token to register in ngrok.yml</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="TunnelDetail"/></returns>
        /// <remarks>
        /// <see cref="RagnarokClient.InitializeAsync"/> will be called if not previously executed
        /// </remarks>
        public static async Task<TunnelDetail> ConnectAsync(this RagnarokClient client, int port, string authToken = null, CancellationToken cancellationToken = default)
            => await ConnectAsync(client, option => option.Address = port.ToString(), authToken, cancellationToken);

        /// <summary>
        /// Start ngrok and open a tunnel
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="tunnelName">Named tunnel configured in ngrok.yml</param>
        /// <param name="authToken">Authorization token to register in ngrok.yml</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="TunnelDetail"/></returns>
        /// <remarks>
        /// The name provided must exist in the ngrok configuration file. <br/>
        /// <see cref="RagnarokClient.InitializeAsync"/> will be called if not previously executed
        /// </remarks>
        public static async Task<TunnelDetail> ConnectAsync(this RagnarokClient client, string tunnelName, 
                                                            string authToken = null, CancellationToken cancellationToken = default)
        {
            var definition = client.Config.Tunnels.FirstOrDefault(x => x.Key.Equals(tunnelName, StringComparison.OrdinalIgnoreCase));

            if (definition.Equals(default(KeyValuePair<string, TunnelDefinition>))) 
                throw new NgrokConfigurationException($"Could not find config for named tunnel {tunnelName}");

            definition.Value.Name = definition.Key;

            return await ConnectAsync(client, definition.Value, authToken, cancellationToken);
        }

        /// <summary>
        /// Start ngrok and open a tunnel
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="details"><see cref="TunnelDetail"/> to replicate</param>
        /// <param name="authToken">Authorization token to register in ngrok.yml</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <remarks>
        /// Creates a new tunnel based on properties defined in the provided <see cref="TunnelDetail"/> <br/>
        /// <see cref="RagnarokClient.InitializeAsync"/> will be called if not previously executed
        /// </remarks>
        public static async Task<TunnelDetail> ConnectAsync(this RagnarokClient client, TunnelDetail details, 
                                                            string authToken = null, CancellationToken cancellationToken = default)
        {
            return await ConnectAsync(client, options =>
            {
                options.Name = details.Name;
                options.Protocol = details.Proto;
                options.Address = details.Config.Address;
                options.BindTLS = details.Proto == TunnelProtocol.HTTP ? BindTLS.False : BindTLS.True;
            }, 
            authToken: authToken,
            cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Start ngrok and open a tunnel
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="options">Options used to define the tunnel to be created</param>
        /// <param name="authToken">Authorization token to register in ngrok.yml</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="TunnelDetail"/></returns>
        /// <remarks>
        /// <see cref="RagnarokClient.InitializeAsync"/> will be called if not previously executed
        /// </remarks>
        public static async Task<TunnelDetail> ConnectAsync(this RagnarokClient client, Action<TunnelDefinition> options,
                                                            string authToken = null, CancellationToken cancellationToken = default)
        {
            var definition = new TunnelDefinition();
            options?.Invoke(definition);

            return await ConnectAsync(client, definition, authToken, cancellationToken);
        }

        /// <summary>
        /// Start ngrok and open a tunnel
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="options">Options used to define the tunnel to be created</param>
        /// <param name="authToken">Authorization token to register in ngrok.yml</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="TunnelDetail"/></returns>
        /// <remarks>
        /// <see cref="RagnarokClient.InitializeAsync"/> will be called if not previously executed
        /// </remarks>
        public static async Task<TunnelDetail> ConnectAsync(this RagnarokClient client, TunnelDefinition options, 
                                                            string authToken = null, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(authToken)) await client.RegisterAuthTokenAsync(authToken);
            if (!client.Ngrok.IsActive) await client.InitializeAsync();

            options ??= new TunnelDefinition();

            return await client.StartTunnelAsync(options, cancellationToken);
        }

        /// <summary>
        /// Close an open tunnel
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="url">The <see cref="TunnelDetail.PublicURL"/> of the tunnel to stop</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <remarks>
        /// If <paramref name="url"/> is not provided, all tunnels will be stopped
        /// </remarks>
        public static async Task<bool> DisconnectAsync(this RagnarokClient client, string url = null, CancellationToken cancellationToken = default)
        {
            if (!client.Ngrok.IsActive) return false;

            var tunnels = await client.ListTunnelsAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(url)) return await client.DisconnectAllAsync(tunnels, cancellationToken: cancellationToken);

            var detail = tunnels.FirstOrDefault(x => x.PublicURL.Equals(url, StringComparison.OrdinalIgnoreCase));

            if (detail == null) return false;

            await client.StopTunnelAsync(detail.Name, cancellationToken);

            return true;
        }

        /// <summary>
        /// Close all specified open tunnels
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="tunnels">List of <see cref="TunnelDetail"/>s to be stopped</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns></returns>
        private static async Task<bool> DisconnectAllAsync(this RagnarokClient client, IEnumerable<TunnelDetail> tunnels, CancellationToken cancellationToken = default)
        {
            if (!tunnels.Any()) return false;

            foreach (var tunnel in tunnels)
                await client.StopTunnelAsync(tunnel.Name, cancellationToken);

            return true;
        }

        /// <summary>
        /// Add/Modify the authtoken property in the ngrok configuration file
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="authToken">ngrok authtoken <see href="https://dashboard.ngrok.com/get-started/your-authtoken"/></param>
        /// <param name="throwOnError">Should exceptions be thrown or suppressed</param>
        /// <returns></returns>
        public static async Task<bool> RegisterAuthTokenAsync(this RagnarokClient client, string authToken, bool throwOnError = true)
        {
            var processData = new StringBuilder();
            var startInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = client.Options.NgrokExecutablePath,
                Arguments = $"authtoken {authToken} --config={client.Options.NgrokConfigPath}"
            };

            var process = new Process() { StartInfo = startInfo };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data)) processData.AppendLine(e.Data);
            };

            process.OutputDataReceived += (sender, e) =>
            {                
                if (!string.IsNullOrWhiteSpace(e.Data)) processData.AppendLine(e.Data);
            };

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            await process.WaitForExitAsync();

            var data = processData.ToString();

            if (string.IsNullOrWhiteSpace(data)) return false;

            var error = data.Contains("ERROR:");

            if (throwOnError && error) throw new Exception(data);
            else if (error) return false;

            return true;
        }

        internal static async Task<bool> WaitForEventAsync(this RagnarokClient client, string eventName, TimeSpan timeout) 
        {
            var type = client.GetType();
            var eventInfo = type.GetEvent(eventName);

            var delay = Task.Delay(timeout);
            var promise = new TaskCompletionSource<bool>();

            EventHandler handler = (object sender, EventArgs args) => promise.SetResult(true); 

            eventInfo.AddEventHandler(client, handler);

            var tsk = await Task.WhenAny(promise.Task, delay);

            if (tsk.Equals(delay)) promise.SetResult(false);

            eventInfo.RemoveEventHandler(client, handler);

            return await promise.Task;
        }
    }
}
