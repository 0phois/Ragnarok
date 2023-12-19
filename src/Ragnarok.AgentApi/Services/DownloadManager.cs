using Ragnarok.AgentApi.Helpers;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Ragnarok.AgentApi.Services
{
    /// <summary>
    /// Manages the downloading of the ngrok executable file
    /// </summary>
    public sealed class DownloadManager
    {
        private const string CDN = "https://bin.equinox.io";
        private const string CDN_Path = "/c/bNyj1mQVY4c/ngrok-v3-stable-";

        private readonly HttpClient _httpClient = new();
        private string DownloadUrl { get; } = $"{CDN}/{CDN_Path}-{RuntimeHelper.GetOsArchitectureString()}.zip";

        /// <summary>
        /// Create a new instance of the download manager
        /// </summary>
        public DownloadManager() { }

        /// <summary>
        /// Download ngrok executable from equinox.io
        /// </summary>
        /// <param name="downloadPath">Ngrok executable will be downloaded to the path specified</param>
        /// <param name="cancellationToken">Used to propagate a notification to cancel to operation</param>
        /// <returns></returns>
        public async Task DownloadNgrokAsync(string downloadPath, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(DownloadUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            var fileName = $"{RuntimeHelper.GetOsArchitectureString()}.zip";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            var downloadStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            await using (var writer = File.Create(filePath))
                await downloadStream.CopyToAsync(writer, cancellationToken);

            ZipFile.ExtractToDirectory(filePath, Path.GetDirectoryName(downloadPath), overwriteFiles: true);

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) GrantFileExecutablePermissions(downloadPath);

            if (File.Exists(filePath)) File.Delete(filePath);
        }

        private static void GrantFileExecutablePermissions(string path)
        {
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "/bin/bash",
                Arguments = $@"-c ""chmod +x {path}/ngrok"""
            };

            var process = new Process() { StartInfo = startInfo };

            process.Start();
            process.WaitForExit();
        }
    }
}
