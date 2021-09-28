using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ragnarok.AgentApi;
using Ragnarok.AgentApi.Exceptions;
using Ragnarok.AgentApi.Extensions;
using Ragnarok.AgentApi.Models;
using Ragnarok.Test.Logging;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace Ragnarok.Test
{
    [Order(19)]
    public class TestAgentApiRequests : IDisposable, IClassFixture<TestSetup>
    {
        private RagnarokClient Ragnarok { get; }
        private HttpListener Listener { get; }
        private readonly ILogger _logger;
        private readonly ITestOutputHelper _testOutputHelper;

        public TestAgentApiRequests(TestSetup setup, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            var httpClient = new HttpClient();
            _logger = XUnitLogger.CreateLogger<RagnarokClient>(_testOutputHelper);
            var optionAccessor = setup.ServiceProvider.GetService<IOptions<RagnarokOptions>>();

            Ragnarok = new RagnarokClient(httpClient, optionAccessor, _logger);
            Listener = new HttpListener();
            Listener.Prefixes.Add("http://localhost:8888/");
        }

        [Fact]
        [Order(20)]
        public async Task ListCapturedRequests()
        {
            #region arrange
            var client = new HttpClient();
            var tunnel = await Ragnarok.ConnectAsync(options => { options.Address = "8888"; options.Inspect = true; });
            var task = Task.Run(() => ListenAsync());

            client.BaseAddress = new Uri(tunnel.PublicURL);
            await client.GetAsync("");
            #endregion

            #region act
            var details = await Ragnarok.ListCapturedRequestsAsync(options => options.Limit = 10);
            StopListener();
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.NotEmpty(details);
            #endregion
        }

        [Fact]
        [Order(21)]
        public async Task ListCapturedRequests_Without_Initializing()
        {
            #region arrange
            #endregion

            #region act
            Task act() => Ragnarok.ListCapturedRequestsAsync(options => options.Limit = 10);
            #endregion

            #region assert
            await Assert.ThrowsAsync<NotInitializedException>(act);
            #endregion
        }

        [Fact]
        [Order(22)]
        public async Task GetCapturedRequest()
        {
            #region arrange
            var client = new HttpClient();
            var tunnel = await Ragnarok.ConnectAsync(options => { options.Address = "8888"; options.Inspect = true; });
            var task = Task.Run(() => ListenAsync());

            client.BaseAddress = new Uri(tunnel.PublicURL);
            await client.GetAsync("");

            var details = await Ragnarok.ListCapturedRequestsAsync(options => options.Limit = 10);
            var requestId = details.First().Id;
            #endregion

            #region act
            var request = await Ragnarok.GetCapturedRequestDetailAsync(requestId);

            StopListener();
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.NotNull(request);
            Assert.Equal(requestId, request.Id);
            #endregion
        }

        [Fact]
        [Order(23)]
        public async Task GetCapturedRequest_InvalidID()
        {
            #region arrange
            var client = new HttpClient();
            var tunnel = await Ragnarok.ConnectAsync(options => { options.Address = "8888"; options.Inspect = true; });
            var task = Task.Run(() => ListenAsync());

            client.BaseAddress = new Uri(tunnel.PublicURL);
            await client.GetAsync("");
            #endregion

            #region act
            StopListener();

            Task act() => Ragnarok.GetCapturedRequestDetailAsync("invalid_id");
            #endregion

            #region assert
            await Assert.ThrowsAsync<NgrokApiException>(act);
            #endregion
        }

        [Fact]
        [Order(24)]
        public async Task ReplayCapturedRequest()
        {
            #region arrange
            var client = new HttpClient();
            var tunnel = await Ragnarok.ConnectAsync(options => { options.Address = "8888"; options.Inspect = true; });
            var task = Task.Run(() => ListenAsync());

            client.BaseAddress = new Uri(tunnel.PublicURL);
            await client.GetAsync("");

            var details = await Ragnarok.ListCapturedRequestsAsync(options => options.Limit = 10);
            var requestId = details.First().Id;
            #endregion

            #region act
            var replayed = await Ragnarok.ReplayCapturedRequestsAsync(requestId);

            await Task.Delay(300);

            StopListener();
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.True(replayed);
            #endregion
        }

        [Fact]
        [Order(25)]
        public async Task ReplayCapturedRequest_InvalidID()
        {
            #region arrange
            var client = new HttpClient();
            var tunnel = await Ragnarok.ConnectAsync(options => { options.Address = "8888"; options.Inspect = true; });
            var task = Task.Run(() => ListenAsync());

            client.BaseAddress = new Uri(tunnel.PublicURL);
            await client.GetAsync("");
            #endregion

            #region act
            StopListener();

            Task act() => Ragnarok.ReplayCapturedRequestsAsync("invalid_id");
            #endregion

            #region assert
            await Assert.ThrowsAsync<NgrokApiException>(act);
            #endregion
        }

        [Fact]
        [Order(26)]
        public async Task DeleteCapturedRequest()
        {
            #region arrange
            var client = new HttpClient();
            var tunnel = await Ragnarok.ConnectAsync(options => { options.Address = "8888"; options.Inspect = true; });
            var task = Task.Run(() => ListenAsync());

            client.BaseAddress = new Uri(tunnel.PublicURL);
            await client.GetAsync("");

            var details = await Ragnarok.ListCapturedRequestsAsync(options => options.Limit = 10);
            var requestId = details.First().Id;
            #endregion

            #region act
            var deleted = await Ragnarok.DeleteCapturedRequestsAsync();

            StopListener();
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.True(deleted);
            #endregion
        }

        [Fact]
        [Order(27)]
        public async Task DeleteCapturedRequest_NoRequests()
        {
            #region arrange
            var client = new HttpClient();
            var tunnel = await Ragnarok.ConnectAsync(options => { options.Address = "8888"; options.Inspect = true; });
            #endregion

            #region act
            var deleted = await Ragnarok.DeleteCapturedRequestsAsync();
            #endregion

            #region assert
            Assert.True(deleted);
            #endregion
        }

        private async Task ListenAsync()
        {
            Listener.Start();

            while (true)
            {
                _ = await Listener.GetContextAsync();
            }
        }

        private void StopListener()
        {
            Listener.Stop();
            Listener.Close();
        }

        public void Dispose() => Ragnarok.StopNgrokProcess();
    }

}
