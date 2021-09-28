using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ragnarok.AgentApi;
using Ragnarok.AgentApi.Exceptions;
using Ragnarok.AgentApi.Extensions;
using Ragnarok.AgentApi.Models;
using Ragnarok.Test.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace Ragnarok.Test
{
    [Order(19)]
    public class TestClientExtensions : IDisposable, IClassFixture<TestSetup>
    {
        private RagnarokClient Ragnarok { get; }
        private readonly ITestOutputHelper _testOutputHelper;

        public TestClientExtensions(TestSetup setup, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            var httpClient = new HttpClient();
            var logger = XUnitLogger.CreateLogger<RagnarokClient>(_testOutputHelper);
            var optionAccessor = setup.ServiceProvider.GetService<IOptions<RagnarokOptions>>();

            Ragnarok = new RagnarokClient(httpClient, optionAccessor, logger);
        }

        [Fact]
        [Order(20)]
        public async Task Connect()
        {
            #region arrange
            foreach (var process in Process.GetProcessesByName("ngrok"))
                process.Kill();
            #endregion

            #region act
            var detail = await Ragnarok.ConnectAsync();
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.NotNull(detail);
            Assert.Equal(AppDomain.CurrentDomain.FriendlyName, detail.Name);
            Assert.Equal(TunnelProtocol.HTTPS, detail.Proto);
            Assert.Equal("http://localhost:80", detail.Config.Address);
            #endregion
        }

        [Fact]
        [Order(21)]
        public async Task Connect_With_Token()
        {
            #region arrange
            foreach (var process in Process.GetProcessesByName("ngrok"))
                process.Kill();
            #endregion

            #region act
            var token = File.ReadAllText("./AuthToken.key");
            var detail = await Ragnarok.ConnectAsync(authToken: token);
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.NotNull(detail);
            Assert.Equal(AppDomain.CurrentDomain.FriendlyName, detail.Name);
            Assert.Equal(TunnelProtocol.HTTPS, detail.Proto);
            Assert.Equal("http://localhost:80", detail.Config.Address);
            #endregion
        }

        [Fact]
        [Order(22)]
        public async Task Connect_To_Port()
        {
            #region arrange
            foreach (var process in Process.GetProcessesByName("ngrok"))
                process.Kill();
            #endregion

            #region act
            var detail = await Ragnarok.ConnectAsync(5050);
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.NotNull(detail);
            Assert.Equal(AppDomain.CurrentDomain.FriendlyName, detail.Name);
            Assert.Equal(TunnelProtocol.HTTPS, detail.Proto);
            Assert.Equal("http://localhost:5050", detail.Config.Address);
            #endregion
        }

        [Fact]
        public async Task Connect_To_Port_With_Token()
        {
            #region arrange
            foreach (var process in Process.GetProcessesByName("ngrok"))
                process.Kill();
            #endregion

            #region act
            var token = File.ReadAllText("./AuthToken.key");
            var detail = await Ragnarok.ConnectAsync(5555, token);
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.NotNull(detail);
            Assert.Equal(AppDomain.CurrentDomain.FriendlyName, detail.Name);
            Assert.Equal(TunnelProtocol.HTTPS, detail.Proto);
            Assert.Equal("http://localhost:5555", detail.Config.Address);
            #endregion
        }

        [Fact]
        [Order(23)]
        public async Task Connect_To_Named_Tunnel()
        {
            #region arrange
            foreach (var process in Process.GetProcessesByName("ngrok"))
                process.Kill();
            #endregion

            #region act
            var detail = await Ragnarok.ConnectAsync(tunnelName: "myAppName");
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.NotNull(detail);
            Assert.Equal("myappname", detail.Name);
            Assert.Equal(TunnelProtocol.HTTPS, detail.Proto);
            Assert.Equal("http://localhost:8888", detail.Config.Address);
            #endregion
        }

        [Fact]
        [Order(24)]
        public async Task Connect_To_Invalid_Named_Tunnel()
        {
            #region arrange
            foreach (var process in Process.GetProcessesByName("ngrok"))
                process.Kill();
            #endregion

            #region act
            Task act() => Ragnarok.ConnectAsync(tunnelName: "AppName");
            #endregion

            #region assert
            await Assert.ThrowsAsync<NgrokConfigurationException>(act);
            #endregion
        }

        [Fact]
        public async Task Connect_With_Options()
        {
            #region arrange
            foreach (var process in Process.GetProcessesByName("ngrok"))
                process.Kill();
            #endregion

            #region act
            var credentials = new AuthenticationCredentials() { Username = "bob", Password = "passw0rd" };
            var detail = await Ragnarok.ConnectAsync(options => { options.Name = "secure"; options.Address = "5432"; options.Auth = credentials; });
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.NotNull(detail);
            Assert.Equal("secure", detail.Name);
            Assert.Equal(TunnelProtocol.HTTPS, detail.Proto);
            Assert.Equal("http://localhost:5432", detail.Config.Address);
            #endregion
        }

        public void Dispose() => Ragnarok.StopNgrokProcess();
    }
}
