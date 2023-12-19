using Ragnarok.AgentApi.Exceptions;
using Ragnarok.AgentApi.Models;
using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Ragnarok.AgentApi
{
    public partial class RagnarokClient
    {
        private NgrokAgentApi AgentApi { get; }

        /// <summary>
        /// Dynamically starts a new tunnel on the ngrok client. 
        /// </summary>
        /// <param name="request"><see cref="TunnelDefinition"/> properties to use when creating the tunnel</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="TunnelDetail"/> describing the started tunnel</returns>
        public Task<TunnelDetail> StartTunnelAsync(TunnelDefinition request, CancellationToken cancellationToken = default)
            => Ngrok.IsActive ? AgentApi.StartTunnelAsync(request, cancellationToken) : throw new NotInitializedException();

        /// <summary>
        /// Get status and metrics about the named running tunnel
        /// </summary>
        /// <param name="name">The name of the running tunnel</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="TunnelDetail"/> describing the started tunnel</returns>
        public Task<TunnelDetail> GetTunnelDetailAsync(string name, CancellationToken cancellationToken = default)
            => Ngrok.IsActive ? AgentApi.GetTunnelDetailAsync(name, cancellationToken) : throw new NotInitializedException();

        /// <summary>
        /// Retrieve a list of running tunnels with status and metrics information.
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>An array of <see cref="TunnelDetail"/></returns>
        public Task<ImmutableArray<TunnelDetail>> ListTunnelsAsync(CancellationToken cancellationToken = default)
            => Ngrok.IsActive ? AgentApi.ListTunnelsAsync(cancellationToken) : throw new NotInitializedException();

        /// <summary>
        /// Stop a running tunnel.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        public Task StopTunnelAsync(string name, CancellationToken cancellationToken = default)
            => Ngrok.IsActive ? AgentApi.StopTunnelAsync(name, cancellationToken) : Task.CompletedTask;

        /// <summary>
        /// Retrieve a list of all HTTP requests captured for inspection. <br/>
        /// <i><see cref="TunnelDefinition.Inspect"/> must be enabled</i>
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>An array of captured <see cref="RequestDetail"/></returns>
        /// <remarks>
        /// This will only return requests that are still in memory.<br/>
        /// <i>ngrok evicts captured requests when their memory usage exceeds inspect_db_size </i>
        /// </remarks>
        public Task<ImmutableArray<RequestDetail>> ListCapturedRequestsAsync(Action<RequestParameters> options, CancellationToken cancellationToken = default)
            => Ngrok.IsActive ? AgentApi.ListCapturedRequestsAsync(options, cancellationToken) : throw new NotInitializedException();

        /// <summary>
        /// Retrieve metadata and raw bytes of a captured request. 
        /// The raw data is base64-encoded in the JSON response. 
        /// </summary>
        /// <param name="requestId">The id of the request to retrieve</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="RequestDetail"/></returns>
        /// <remarks>
        /// The response value maybe null if the local server has not yet responded to a request.
        /// </remarks>
        public Task<RequestDetail> GetCapturedRequestDetailAsync(string requestId, CancellationToken cancellationToken = default)
            => Ngrok.IsActive ? AgentApi.GetCapturedRequestDetailAsync(requestId, cancellationToken) : throw new NotInitializedException();

        /// <summary>
        /// Replays a request against the local endpoint of a tunnel
        /// </summary>
        /// <param name="requestId">The id of request to replay</param>
        /// <param name="tunnelName">The name of the tunnel to play the request against. 
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// If unspecified, the request is played against the same tunnel it was recorded on</param>
        public Task<bool> ReplayCapturedRequestAsync(string requestId, string tunnelName = null, CancellationToken cancellationToken = default)
            => Ngrok.IsActive ? AgentApi.ReplayCapturedRequestsAsync(requestId, tunnelName, cancellationToken) : throw new NotInitializedException();

        /// <summary>
        /// Deletes all captured requests
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        public Task<bool> DeleteCapturedRequestsAsync(CancellationToken cancellationToken = default)
            => Ngrok.IsActive ? AgentApi.DeleteCapturedRequestsAsync(cancellationToken) : throw new NotInitializedException();
    }
}
