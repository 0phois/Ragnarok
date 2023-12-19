using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ragnarok.AgentApi.Exceptions;
using Ragnarok.AgentApi.Extensions;
using Ragnarok.AgentApi.Helpers;
using Ragnarok.AgentApi.Models;
using Ragnarok.AgentApi.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using static Microsoft.Extensions.Options.Options;

namespace Ragnarok.AgentApi
{
    /// <summary>
    /// ngrok Agent Api wrapper
    /// </summary>
    public partial class RagnarokClient : IDisposable
    {
        private const string BaseURL = "http://127.0.0.1:4040";

        internal NgrokProcessManager Ngrok { get; }

        /// <summary>
        /// Configuration details as defined in the ngrok.yml file
        /// </summary>
        public NgrokConfiguration Config { get; }

        /// <summary>
        /// Options for modifying the behavior of the Ragnarok client
        /// </summary>
        public RagnarokOptions Options { get; }

        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        /// <param name="logger"><see cref="ILogger"/> instance for logging <see cref="RagnarokClient"/></param>
        public RagnarokClient(ILogger<RagnarokClient> logger = null) : this(new HttpClient(), logger: logger) { }

        /// <param name="httpClient"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public RagnarokClient(HttpClient httpClient, IOptions<RagnarokOptions> options = null, ILogger<RagnarokClient> logger = null)
        {
            _logger = logger;
            _httpClient = httpClient;

            options ??= Create(new RagnarokOptions());

            Options = ConfigureOptions(options.Value);
            Config = GetNgrokConfiguration(Options.NgrokConfigPath);

            _httpClient.BaseAddress = GetBaseAddress(Config.WebAddress);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Ngrok = new NgrokProcessManager(Config, _logger);
            AgentApi = new NgrokAgentApi(_httpClient, Config.ApiKey, _logger) { ThrowOnError = Options.ThrowOnError };

        }

        /// <summary>
        /// Associates the current application with an instance of an ngrok process
        /// </summary>
        /// <returns>The process id of the associated ngrok instance</returns>
        /// <remarks>
        /// Utilizes an active instances of ngrok or spawns a new ngrok process if none exists <br/>
        /// <i>This action does not establish a tunnel connection</i>
        /// </remarks>
        public async Task<int> InitializeAsync()
        {
            var process = Process.GetProcessesByName("ngrok").FirstOrDefault();

            if (process == null) await StartNgrokProcess();
            else Ngrok.AttachProcess(process);

            Ngrok.Process.Exited += (object sender, EventArgs e) => OnTerminated(Ngrok.Process);

            return Ngrok.Process.Id;
        }

        private async Task StartNgrokProcess()
        {
            await VerifyExecutable();
            await Ngrok.StartProcessAsync(GetProcessStartInfo());

            RedirectProcesOutput();
            await ConnectionDelayAsync();
        }

        private async Task ConnectionDelayAsync()
        {
            if (Config is not null && Config.LogLevel > NgrokConfigLogLevel.info)
            {
                _logger?.LogWarning("Unable to wait for connection event. Delaying 1s...Enable Log_Level info for more accurate connection wait times");
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            else
            {
                var connected = await this.WaitForEventAsync("Connected", TimeSpan.FromMilliseconds(Options.TimeoutMilliseconds));
                if (!connected) throw new TimeoutException($"ngrok failed to establish a connection in the specified time {Options.TimeoutMilliseconds}");
            }
        }

        private async Task VerifyExecutable()
        {
            if (File.Exists(Options.NgrokExecutablePath)) return;

            if (!Options.DownloadNgrok)
                throw new FileNotFoundException("Unable to locate ngrok executable at the path specified. Consider enabling DownloadNgrok in RagnarokOptions",
                                                Options.NgrokExecutablePath);

            _logger?.LogInformation("ngrok executable not found at {Location}. Downloading ngrok...", Options.NgrokExecutablePath);

            await DownloadExecutableAsync();
        }

        private async Task DownloadExecutableAsync()
        {
            if (File.Exists(Options.NgrokExecutablePath)) return;

            var downloadManager = new DownloadManager();
            await downloadManager.DownloadNgrokAsync(Options.NgrokExecutablePath);

            _logger?.LogInformation("Successfully downloaded ngrok -> {Location}", Options.NgrokExecutablePath);
        }

        private ProcessStartInfo GetProcessStartInfo()
        {
            var showConsole = Config is null || Config.ConsoleUI == ConsoleOptions.@true;
            var stdout = Config?.Log.Equals("stdout", StringComparison.OrdinalIgnoreCase) ?? false;
            var stderr = Config?.Log.Equals("stderr", StringComparison.OrdinalIgnoreCase) ?? false;

            if (showConsole && (stdout || stderr))
                throw new NgrokConfigurationException("Console UI connot be enabled if logging stdout or stderr");

            var arguments = new StringBuilder("start --none")
                                      .Append(" --config=").Append(Options.NgrokConfigPath);

            if (stdout) arguments.Append(" --log=stdout");
            if (stderr) arguments.Append(" --log=stderr");

            return new ProcessStartInfo
            {
                UseShellExecute = showConsole,
                RedirectStandardError = !showConsole,
                RedirectStandardOutput = !showConsole,
                FileName = Options.NgrokExecutablePath,
                WorkingDirectory = Environment.CurrentDirectory,
                WindowStyle = showConsole ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
                Arguments = arguments.ToString().ToLower()
            };
        }

        private static Uri GetBaseAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) address = BaseURL;
            else if (address.Equals("false", StringComparison.OrdinalIgnoreCase))
                throw new NgrokConfigurationException("Setting 'web_addr:false' is not supported");

            if (!address.StartsWith("http", StringComparison.OrdinalIgnoreCase)) address = address.Insert(0, "http://");

            return new Uri(address);
        }

        private static NgrokConfiguration GetNgrokConfiguration(string path)
        {
            path ??= DirectoryHelper.DefaultConfigPath();

            if (!File.Exists(path))
            {
                File.Create(path).Close();
                return new();
            }

            using StreamReader reader = File.OpenText(path);
            var deserializer = new DeserializerBuilder().Build();
            return deserializer.Deserialize<NgrokConfiguration>(reader);
        }

        private static RagnarokOptions ConfigureOptions(RagnarokOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.NgrokConfigPath))
                options.NgrokConfigPath = DirectoryHelper.DefaultConfigPath();

            if (string.IsNullOrWhiteSpace(options.NgrokExecutablePath))
                options.NgrokExecutablePath = DirectoryHelper.DefaultExecutablePath();

            return options;
        }

        /// <summary>
        /// Stop the attached ngrok process
        /// </summary>
        /// <remarks>
        /// Only stops the attached instance if it were spawned by the application. <br/>
        /// Use <see cref="KillNgrokProcess"/> to terminate attached instances that were created externally.
        /// </remarks>
        public void StopNgrokProcess() => Ngrok.StopProcess();

        /// <summary>
        /// Terminate the attached ngrok process
        /// </summary>
        public void KillNgrokProcess() => Ngrok.KillProcess();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            Ngrok?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
