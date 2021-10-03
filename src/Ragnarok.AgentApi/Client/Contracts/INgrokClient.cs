using Ragnarok.AgentApi.Models;
using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Ragnarok.AgentApi.Contracts
{
    internal interface INgrokClient
    {
		/// <summary>
		/// Dynamically starts a new tunnel on the ngrok client
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
		/// <returns></returns>
		Task<TunnelDetail> StartTunnelAsync(TunnelDefinition request, CancellationToken cancellationToken = default);

		/// <summary>
		/// Returns a list of running tunnels with status and metrics information
		/// </summary>
		/// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
		/// <returns></returns>
		Task<ImmutableArray<TunnelDetail>> ListTunnelsAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Get status and metrics about the named running tunnel
		/// </summary>
		/// <param name="name">Name of the tunnel to retrieve details for</param>
		/// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
		/// <returns></returns>
		Task<TunnelDetail> GetTunnelDetailAsync(string name, CancellationToken cancellationToken = default);

		/// <summary>
		/// Stop a running tunnel
		/// </summary>
		/// <param name="name">Name of the tunnel to stop</param>
		/// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
		/// <returns></returns>
		Task StopTunnelAsync(string name, CancellationToken cancellationToken = default);

		/// <summary>
		/// Returns a list of all HTTP requests captured for inspection.
		/// This will only return requests that are still in memory.
		/// </summary>
		/// <param name="options"> Query parameter options (limit, tunnel_name)</param>
		/// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
		/// <returns></returns>
		/// <remarks>
		/// ngrok evicts captured requests when their memory usage exceeds <see cref="NgrokConfiguration.InsepectDatabaseByteSize"/>
		/// </remarks>
		Task<ImmutableArray<RequestDetail>> ListCapturedRequestsAsync(Action<RequestParameters> options, CancellationToken cancellationToken = default);

		/// <summary>
		/// Replays a request against the local endpoint of a tunnel
		/// </summary>
		/// <param name="requestId">Id of the request to replay</param>
		/// <param name="tunnelName">Name of the tunnel to play the request against. 
		/// If unspecified, the request is played against the same tunnel it was recorded on</param>
		/// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
		/// <returns></returns>
		Task<bool> ReplayCapturedRequestsAsync(string requestId = null, string tunnelName =null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes all captured requests
		/// </summary>
		/// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
		/// <returns></returns>
		Task<bool> DeleteCapturedRequestsAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Returns metadata and raw bytes of a captured request. 
		/// The raw data is base64-encoded in the JSON response.
		/// </summary>
		/// <param name="requestId">Id of the request to retrieve</param>
		/// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
		/// <returns></returns>
		/// <remarks>
		/// The response value maybe null if the local server has not yet responded to a request.
		/// </remarks>
		Task<RequestDetail> GetCapturedRequestDetailAsync(string requestId, CancellationToken cancellationToken = default);
	}
}