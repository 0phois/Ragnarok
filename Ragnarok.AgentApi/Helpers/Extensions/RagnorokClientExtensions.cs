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
    public static class RagnorokClientExtensions
    {
        /// <summary>
        /// Start ngrok and open a tunnel
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="authToken">Authorization token to register in ngrok.yml</param>
        /// <returns><see cref="TunnelDetail"/></returns>
        /// <remarks>
        /// <see cref="RagnarokClient.InitializeAsync"/> will be called if not previously executed
        /// </remarks>
        public static async Task<TunnelDetail> ConnectAsync(this RagnarokClient client, string authToken = null, CancellationToken cancellation = default)
            => await ConnectAsync(client, new TunnelDefinition(), authToken, cancellation);

        /// <summary>
        /// Start ngrok and open a tunnel
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="port">Local port number to forward traffic</param>
        /// <param name="authToken">Authorization token to register in ngrok.yml</param>
        /// <returns><see cref="TunnelDetail"/></returns>
        /// <remarks>
        /// <see cref="RagnarokClient.InitializeAsync"/> will be called if not previously executed
        /// </remarks>
        public static async Task<TunnelDetail> ConnectAsync(this RagnarokClient client, int port, string authToken = null, CancellationToken cancellation = default)
            => await ConnectAsync(client, option => option.Address = port.ToString(), authToken, cancellation);

        /// <summary>
        /// Start ngrok and open a tunnel
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="tunnelName">Named tunnel configured in ngrok.yml</param>
        /// <param name="authToken">Authorization token to register in ngrok.yml</param>
        /// <returns><see cref="TunnelDetail"/></returns>
        /// <remarks>
        /// <see cref="RagnarokClient.InitializeAsync"/> will be called if not previously executed
        /// </remarks>
        public static async Task<TunnelDetail> ConnectAsync(this RagnarokClient client, string tunnelName, 
                                                            string authToken = null, CancellationToken cancellation = default)
        {
            var definition = client.Config.Tunnels.FirstOrDefault(x => x.Key.Equals(tunnelName, StringComparison.OrdinalIgnoreCase));

            if (definition.Equals(default(KeyValuePair<string, TunnelDefinition>))) 
                throw new NgrokConfigurationException($"Could not find config for named tunnel {tunnelName}");

            definition.Value.Name = definition.Key;

            return await ConnectAsync(client, definition.Value, authToken, cancellation);
        }

        /// <summary>
        /// Start ngrok and open a tunnel
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="options">Options used to define the tunnel to be created</param>
        /// <param name="authToken">Authorization token to register in ngrok.yml</param>
        /// <returns><see cref="TunnelDetail"/></returns>
        /// <remarks>
        /// <see cref="RagnarokClient.InitializeAsync"/> will be called if not previously executed
        /// </remarks>
        public static async Task<TunnelDetail> ConnectAsync(this RagnarokClient client, Action<TunnelDefinition> options,
                                                            string authToken = null, CancellationToken cancellation = default)
        {
            var definition = new TunnelDefinition();
            options?.Invoke(definition);

            return await ConnectAsync(client, definition, authToken, cancellation);
        }

        /// <summary>
        /// Start ngrok and open a tunnel
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="options">Options used to define the tunnel to be created</param>
        /// <param name="authToken">Authorization token to register in ngrok.yml</param>
        /// <returns><see cref="TunnelDetail"/></returns>
        /// <remarks>
        /// <see cref="RagnarokClient.InitializeAsync"/> will be called if not previously executed
        /// </remarks>
        public static async Task<TunnelDetail> ConnectAsync(this RagnarokClient client, TunnelDefinition options, 
                                                            string authToken = null, CancellationToken cancellation = default)
        {
            if (!string.IsNullOrWhiteSpace(authToken)) await client.RegisterAuthTokenAsync(authToken);
            if (!client.Ngrok.IsActive) await client.InitializeAsync();

            options ??= new TunnelDefinition();

            return await client.StartTunnelAsync(options, cancellation);
        }

        /// <summary>
        /// Close an open tunnel
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="url">The <see cref="TunnelDetail.PublicURL"/> of the tunnel to stop</param>
        /// <remarks>
        /// If <paramref name="url"/> is not provided, all tunnels will be stopped
        /// </remarks>
        public static async Task<bool> DisconnectAsync(this RagnarokClient client, string url = null, CancellationToken cancellation = default)
        {
            if (!client.Ngrok.IsActive) return false;

            var tunnels = await client.ListTunnelsAsync(cancellation);

            if (string.IsNullOrWhiteSpace(url)) return await client.DisconnectAllAsync(tunnels, cancellation: cancellation);

            var detail = tunnels.FirstOrDefault(x => x.PublicURL.Equals(url, StringComparison.OrdinalIgnoreCase));

            if (detail == null) return false;

            await client.StopTunnelAsync(detail.Name, cancellation);

            return true;
        }

        /// <summary>
        /// Close all specified open tunnels
        /// </summary>
        /// <param name="client">An instance of <see cref="RagnarokClient"/></param>
        /// <param name="tunnels">List of <see cref="TunnelDetail"/>s to be stopped</param>
        /// <returns></returns>
        private static async Task<bool> DisconnectAllAsync(this RagnarokClient client, IEnumerable<TunnelDetail> tunnels, CancellationToken cancellation = default)
        {
            if (!tunnels.Any()) return false;

            foreach (var tunnel in tunnels)
                await client.StopTunnelAsync(tunnel.Name, cancellation);

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
