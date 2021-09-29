using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ragnarok.AgentApi;
using Ragnarok.AgentApi.Exceptions;
using Ragnarok.AgentApi.Models;
using Ragnarok.Test.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace Ragnarok.Test
{
    [Order(11)]
    public class TestAgentApiTunnels : IDisposable, IClassFixture<TestSetup>
    {
        private RagnarokClient Ragnarok { get; }
        private readonly ILogger _logger;
        private readonly ITestOutputHelper _testOutputHelper;

        public TestAgentApiTunnels(TestSetup setup, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            var httpClient = new HttpClient();
            _logger = XUnitLogger.CreateLogger<RagnarokClient>(_testOutputHelper);
            var optionAccessor = setup.ServiceProvider.GetService<IOptions<RagnarokOptions>>();

            Ragnarok = new RagnarokClient(httpClient, optionAccessor, _logger);
        }

        [Fact]
        [Order(12)]
        public async Task StartNgrokTunnel()
        {
            #region arrange
            await Ragnarok.InitializeAsync();
            #endregion

            #region act
            var tunnel = new TunnelDefinition()
            {
                Name = "King's Cross", 
                Address = "5975",
                Protocol = TunnelProtocol.HTTP, 
                BindTLS = BindTLS.False
            };
            var detail = await Ragnarok.StartTunnelAsync(tunnel);
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.NotNull(detail);
            Assert.Equal("king's cross", detail.Name);
            Assert.Equal(TunnelProtocol.HTTP, detail.Proto);
            Assert.Equal("http://localhost:5975", detail.Config.Address);
            #endregion
        }

        [Fact]
        [Order(13)]
        public async Task StartNgrokTunnel_Without_Initializing()
        {
            #region arrange
            var tunnel = new TunnelDefinition()
            {
                Name = "King's Cross",
                Address = "5975",
                Protocol = TunnelProtocol.HTTP,
                BindTLS = BindTLS.False
            };
            #endregion

            #region act
            Task act() => Ragnarok.StartTunnelAsync(tunnel);
            #endregion

            #region assert
            await Assert.ThrowsAsync<NotInitializedException>(act);
            #endregion
        }

        [Fact]
        [Order(14)]
        public async Task GetNgrokTunnel()
        {
            #region arrange
            await Ragnarok.InitializeAsync();
            var tunnel = new TunnelDefinition()
            {
                Name = "secure tunnel",
                Address = "5555",
                Protocol = TunnelProtocol.HTTP
            };
            await Ragnarok.StartTunnelAsync(tunnel);
            #endregion

            #region act
            var detail = await Ragnarok.GetTunnelDetailAsync("secure tunnel");
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.NotNull(detail);
            Assert.Equal("secure tunnel", detail.Name);
            Assert.Equal(TunnelProtocol.HTTPS, detail.Proto);
            Assert.Equal("http://localhost:5555", detail.Config.Address);
            #endregion
        }

        [Fact]
        [Order(15)]
        public async Task GetNgrokTunnel_Without_Initializing()
        {
            #region arrange
            #endregion

            #region act
            Task act() => Ragnarok.GetTunnelDetailAsync("scary tunnel");
            #endregion

            #region assert
            await Assert.ThrowsAsync<NotInitializedException>(act);
            #endregion
        }

        [Fact]
        [Order(16)]
        public async Task ListNgrokTunnels()
        {
            #region arrange
            await Ragnarok.InitializeAsync();
            var tunnel = new TunnelDefinition()
            {
                Name = "my tunnels",
                Address = "5555",
                Protocol = TunnelProtocol.HTTP
            };
            await Ragnarok.StartTunnelAsync(tunnel);
            #endregion

            #region act
            var tunnels = await Ragnarok.ListTunnelsAsync();
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.NotEmpty(tunnels);
            Assert.Equal(2, tunnels.Length);
            #endregion
        }

        [Fact]
        [Order(17)]
        public async Task ListNgrokTunnels_WithoutInitializing()
        {
            #region arrange
            #endregion

            #region act
            Task act() => Ragnarok.ListTunnelsAsync();
            #endregion

            #region assert
            await Assert.ThrowsAsync<NotInitializedException>(act);
            #endregion
        }

        [Fact]
        [Order(18)]
        public async Task ListNgrokTunnels_After_StoppingTunnel()
        {
            #region arrange
            await Ragnarok.InitializeAsync(); 
            var tunnel = new TunnelDefinition()
            {
                Name = "railroad",
                Address = "5050",
                Protocol = TunnelProtocol.HTTP,
                BindTLS = BindTLS.False
            };
            await Ragnarok.StartTunnelAsync(tunnel);
            await Task.Delay(500);
            #endregion

            #region act
            await Ragnarok.StopTunnelAsync("railroad");
            Task act() => Ragnarok.GetTunnelDetailAsync("railroad");
            #endregion

            #region assert
            await Assert.ThrowsAsync<NgrokApiException>(act);            
            #endregion
        }

        public void Dispose() => Ragnarok.KillNgrokProcess();
    }

}
