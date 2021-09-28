using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Ragnarok.AgentApi.Contracts;
using Ragnarok.AgentApi.Exceptions;
using Ragnarok.AgentApi.Helpers;
using Ragnarok.AgentApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Ragnarok.AgentApi
{
    internal sealed class NgrokAgentApi : INgrokClient
    {
        private const string TunnelsEndpoint = "/api/tunnels";
        private const string FormattedTunnelsEndpoint = "/api/tunnels/{0}";
        private const string RequestsEndpoint = "api/requests/http";
        private const string FormattedRequestsEndpoint = "api/requests/http/{0}";

        private readonly ILogger _logger;

        private HttpClient Client { get; }
        public bool ThrowOnError { get; init; }

        public NgrokAgentApi(HttpClient client) : this(client, null) { }

        public NgrokAgentApi(HttpClient client, ILogger logger)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Client = client;
            _logger = logger;
        }

        public async Task<TunnelDetail> StartTunnelAsync(TunnelDefinition request, CancellationToken cancellationToken = default)
        {
            AssertNotNull(nameof(request), request == null);
            AssertNotNull(nameof(request.Address), request == null);

            if (request.Protocol == TunnelProtocol.HTTPS) request.Protocol = TunnelProtocol.HTTP;

            bool retry;
            int attempts = 0;
            string json = JsonSerializer.Serialize(request).ToLower();

            _logger.LogDebug("POST: {Base}{Endpoint}={Content}", Client.BaseAddress, TunnelsEndpoint, json);

            using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            do
            {
                retry = attempts++ < 100;

                var response = await Client.PostAsync(TunnelsEndpoint, content, cancellationToken);
                var tunnelDetail = await ParseResponseAsync<TunnelDetail>(response, retry ? 104 : -1, cancellationToken);

                if (tunnelDetail != null) return tunnelDetail;

                await Task.Delay(100, cancellationToken);
            } while (retry);

            return new TunnelDetail();
        }

        public async Task<TunnelDetail> GetTunnelDetailAsync(string name, CancellationToken cancellationToken = default)
        {
            AssertNotNull(nameof(name), string.IsNullOrWhiteSpace(name));

            var request = string.Format(FormattedTunnelsEndpoint, name);

            _logger.LogDebug("GET: {Base}{Endpoint}", Client.BaseAddress, request);

            var response = await Client.GetAsync(request, cancellationToken);
            
            return await ParseResponseAsync<TunnelDetail>(response, cancellationToken: cancellationToken);
        }

        public async Task<ImmutableArray<TunnelDetail>> ListTunnelsAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("GET: {Base}{Endpoint}", Client.BaseAddress, TunnelsEndpoint);

            var response = await Client.GetAsync(TunnelsEndpoint, cancellationToken);
            var details = await ParseResponseAsync<TunnelList>(response, cancellationToken: cancellationToken);

            return details.Tunnels.ToImmutableArray();
        }

        public async Task StopTunnelAsync(string name, CancellationToken cancellationToken = default)
        {
            AssertNotNull(nameof(name), string.IsNullOrWhiteSpace(name));

            var request = string.Format(FormattedTunnelsEndpoint, name);

            _logger.LogDebug("Delete: {Base}{Endpoint}", Client.BaseAddress, request);

            var response = await Client.DeleteAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode) await ParseRequestErrorAsync(response, cancellationToken: cancellationToken);
        }

        public async Task<ImmutableArray<RequestDetail>> ListCapturedRequestsAsync(Action<RequestParameters> options, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("GET: {Base}{Endpoint}", Client.BaseAddress, RequestsEndpoint);

            var parameters = new RequestParameters();
            options?.Invoke(parameters);

            var opts = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            opts.Converters.Add(new StringConverter());

            var json = JsonSerializer.Serialize(parameters, opts).ToLower();

            var queryString = QueryHelpers.AddQueryString(RequestsEndpoint, JsonSerializer.Deserialize<Dictionary<string, string>>(json, opts));
            var response = await Client.GetAsync(queryString, cancellationToken);
            var request = await ParseResponseAsync<CapturedRequest>(response, cancellationToken: cancellationToken);

            return request.Requests.ToImmutableArray();
        }

        public async Task<RequestDetail> GetCapturedRequestDetailAsync(string requestId, CancellationToken cancellationToken = default)
        {
            AssertNotNull(nameof(requestId), string.IsNullOrWhiteSpace(requestId));

            var request = string.Format(FormattedRequestsEndpoint, requestId);

            _logger.LogDebug("GET: {Base}{Endpoint}", Client.BaseAddress, request);

            var response = await Client.GetAsync(request, cancellationToken);

            return await ParseResponseAsync<RequestDetail>(response, cancellationToken: cancellationToken);
        }

        public async Task<bool> ReplayCapturedRequestsAsync(string requestId, string tunnelName = null, CancellationToken cancellationToken = default)
        {
            AssertNotNull(nameof(requestId), string.IsNullOrWhiteSpace(requestId));

            var json = JsonSerializer.Serialize(new { id = requestId, tunnel_name = tunnelName });

            _logger.LogDebug("POST: {Base}{Endpoint}={Content}", Client.BaseAddress, RequestsEndpoint, json);

            using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(RequestsEndpoint, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                await ParseRequestErrorAsync(response, cancellationToken: cancellationToken);
                return true;
            }

            return true;
        }

        public async Task<bool> DeleteCapturedRequestsAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Delete: {Base}{Endpoint}", Client.BaseAddress, RequestsEndpoint);

            var response = await Client.DeleteAsync(RequestsEndpoint, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                await ParseRequestErrorAsync(response, cancellationToken: cancellationToken);
                return false;
            }

            return true;
        }

        private static void AssertNotNull(string param, bool isNull)
        {
            if (isNull) throw new ArgumentNullException(param, "Parameter cannot be null");
        }

        private async Task<T> ParseResponseAsync<T>(HttpResponseMessage response, int suppressErrorCode = -1, CancellationToken cancellationToken = default)
        {
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                _logger.LogDebug("Request executed successfully - Status: {Code}", response.StatusCode);

                return await JsonSerializer.DeserializeAsync<T>(responseStream, cancellationToken: cancellationToken);
            }
            else
            {
                await ParseRequestErrorAsync(response, suppressErrorCode, cancellationToken);
                return default;
            }               
        }

        private async Task ParseRequestErrorAsync(HttpResponseMessage response, int suppressErrorCode = -1, CancellationToken cancellationToken = default)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var error = JsonSerializer.Deserialize<ErrorResponse>(errorContent);

            if (error.NgrokErrorCode == suppressErrorCode) return;

            if (ThrowOnError) throw new NgrokApiException(error.ToString());
        }
    }
}
