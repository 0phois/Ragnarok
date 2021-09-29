using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ragnarok.AgentApi;
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
    [Order(6)]
    public class TestClientInitialization : IDisposable, IClassFixture<TestSetup>
    {
        private RagnarokClient Ragnarok { get; }

        private readonly ITestOutputHelper _testOutputHelper;

        public TestClientInitialization(TestSetup setup, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            var httpClient = new HttpClient();
            var logger = XUnitLogger.CreateLogger<RagnarokClient>(_testOutputHelper);
            var optionAccessor = setup.ServiceProvider.GetService<IOptions<RagnarokOptions>>();

            Ragnarok = new RagnarokClient(httpClient, optionAccessor, logger);
        }

        [Fact]
        [Order(7)]
        public async Task InitializeRagnarok_Without_ActiveNgrokProcess()
        {
            #region arrange
            foreach (var process in Process.GetProcessesByName("ngrok"))
                process.Kill();
            #endregion

            #region act
            var processId = await Ragnarok.InitializeAsync();
            var ngrokProcess = Process.GetProcessById(processId);
            #endregion

            #region assert
            Assert.NotNull(ngrokProcess);
            Assert.False(ngrokProcess.HasExited);
            Assert.True(Ragnarok.Ngrok.IsActive);
            Assert.True(Ragnarok.Ngrok.IsManaged);
            #endregion
        }

        [Fact]
        [Order(9)]
        public async Task InitializeRagnarok__With_ActiveNgrokProcess()
        {
            #region arrange
            var processInfo = new ProcessStartInfo("Ngrok.exe", "start --none")
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory
            };

            var activeProcess = new Process { StartInfo = processInfo };
            activeProcess.Start();
            #endregion

            #region act
            var processId = await Ragnarok.InitializeAsync();
            var ngrokProcess = Process.GetProcessById(processId);
            #endregion

            #region assert
            Assert.NotNull(ngrokProcess);
            Assert.False(ngrokProcess.HasExited);
            Assert.True(Ragnarok.Ngrok.IsActive);
            Assert.False(Ragnarok.Ngrok.IsManaged);            
            #endregion
        }


        [Fact]
        [Order(8)]
        public async Task RegisterAuthToken_Without_ActiveNgrokProcess()
        {
            #region arrange
            var authToken = File.ReadAllText("./Authtoken.key");
            foreach (var process in Process.GetProcessesByName("ngrok"))
                process.Kill();
            #endregion

            #region act
            var registered = await Ragnarok.RegisterAuthTokenAsync(authToken);
            #endregion

            #region assert
            Assert.True(registered);
            #endregion
        }

        [Fact]
        public async Task RegisterAuthToken_With_ActiveNgrokProcess()
        {
            #region arrange
            var authToken = File.ReadAllText("./AuthToken.key");
            await Ragnarok.InitializeAsync();
            #endregion

            #region act
            var registered = await Ragnarok.RegisterAuthTokenAsync(authToken);
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.True(registered);
            #endregion
        }

        [Fact]
        [Order(10)]
        public async Task RegisterAuthToken_With_ActiveTunnelConnection()
        {
            #region arrange
            var definition = new TunnelDefinition();
            var authToken = File.ReadAllText("./Authtoken.key");
            await Ragnarok.InitializeAsync();
            await Ragnarok.StartTunnelAsync(definition);
            #endregion

            #region act
            var registered = await Ragnarok.RegisterAuthTokenAsync(authToken);
            Ragnarok.StopNgrokProcess();
            #endregion

            #region assert
            Assert.True(registered);
            #endregion
        }

        public void Dispose() => Ragnarok.KillNgrokProcess();
    }
}
