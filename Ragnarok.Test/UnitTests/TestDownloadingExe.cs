using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ragnarok.AgentApi;
using Ragnarok.AgentApi.Models;
using Ragnarok.AgentApi.Services;
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
    [Order(1)]
    public class TestDownloadingExe : IDisposable, IClassFixture<TestSetup>
    {
        private RagnarokClient Ragnarok { get; }
        private readonly HttpClient _httpClient;
        private readonly ITestOutputHelper _testOutputHelper;

        public TestDownloadingExe(TestSetup setup, ITestOutputHelper testOutputHelper)
        {
            _httpClient = new HttpClient();
            _testOutputHelper = testOutputHelper;

            var logger = XUnitLogger.CreateLogger<RagnarokClient>(_testOutputHelper);
            var optionAccessor = setup.ServiceProvider.GetService<IOptions<RagnarokOptions>>();

            Ragnarok = new RagnarokClient(_httpClient, optionAccessor, logger);
        }

        [Fact]
        [Order(2)]
        public async Task DownloadExe_Config_True()
        {
            #region arrange
            foreach (var process in Process.GetProcessesByName("ngrok"))
                process.Kill();

            if (File.Exists(Ragnarok.Options.NgrokExecutablePath))
                File.Delete(Ragnarok.Options.NgrokExecutablePath);

            Ragnarok.Options.DownloadNgrok = true;
            #endregion

            #region act
            var downloadManager = new DownloadManager(_httpClient);
            await downloadManager.DownloadNgrokAsync(Ragnarok.Options.NgrokExecutablePath);
            #endregion

            #region assert
            Assert.True(File.Exists(Ragnarok.Options.NgrokExecutablePath));
            #endregion
        }

        [Fact]
        [Order(3)]
        public async Task DownloadExe_Config_False()
        {
            #region arrange
            foreach (var process in Process.GetProcessesByName("ngrok"))
                process.Kill();

            if (File.Exists(Ragnarok.Options.NgrokExecutablePath))
                File.Delete(Ragnarok.Options.NgrokExecutablePath);

            Ragnarok.Options.DownloadNgrok = false;
            #endregion

            #region act
            var downloadManager = new DownloadManager(_httpClient);
            await downloadManager.DownloadNgrokAsync(Ragnarok.Options.NgrokExecutablePath);
            #endregion

            #region assert
            Assert.True(File.Exists(Ragnarok.Options.NgrokExecutablePath));
            #endregion
        }

        [Fact]
        [Order(4)]
        public async Task InitializeRagnarok_DownloadConfig_True()
        {
            #region arrange
            foreach (var process in Process.GetProcessesByName("ngrok"))
                process.Kill();

            if (File.Exists(Ragnarok.Options.NgrokExecutablePath))
                File.Delete(Ragnarok.Options.NgrokExecutablePath);

            Ragnarok.Options.DownloadNgrok = true;
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
            Assert.True(File.Exists(Ragnarok.Options.NgrokExecutablePath));
            #endregion
        }

        [Fact]
        [Order(5)]
        public async Task InitializeRagnarok_DownloadConfig_False()
        {
            #region arrange
            foreach (var process in Process.GetProcessesByName("ngrok"))
                process.Kill();

            if (File.Exists(Ragnarok.Options.NgrokExecutablePath))
                File.Delete(Ragnarok.Options.NgrokExecutablePath);

            Ragnarok.Options.DownloadNgrok = false;
            #endregion

            #region act
            Task act() => Ragnarok.InitializeAsync();
            #endregion

            #region assert
            await Assert.ThrowsAsync<FileNotFoundException>(act);
            #endregion
        }

        public void Dispose() => Ragnarok.KillNgrokProcess();
    }
}
