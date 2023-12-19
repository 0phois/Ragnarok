using Ragnarok.AgentApi.Helpers;

namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// Options for managing the <see cref="RagnarokClient"/>
    /// </summary>
    public class RagnarokOptions
    {
        /// <summary>
        /// Use for binding the class to a configuration provider
        /// </summary>
        public const string Ragnarok = "Ragnarok";

        /// <summary>
        /// Custom path to the ngrok config file. <br/>
        /// Reference: <see href="https://ngrok.com/docs#config-location">https://ngrok.com/docs#config-location</see>
        /// </summary>
        /// <remarks>
        /// Defaults to ngrok default [$HOME/AppData/Local/ngrok/ngrok.yml]
        /// </remarks>
        public string NgrokConfigPath { get; set; } = DirectoryHelper.DefaultConfigPath();

        /// <summary>
        /// Custom path to the ngrok executable.
        /// </summary>
        /// <remarks>
        /// Defaults to the project directory.
        /// </remarks>
        public string NgrokExecutablePath { get; set; } = DirectoryHelper.DefaultExecutablePath();

        /// <summary>
        /// Download ngrok if not found in the <see cref="NgrokExecutablePath"/>. 
        /// </summary>
        /// <remarks>
        /// Downloaded executable will be placed in the <see cref="NgrokExecutablePath"/>
        /// </remarks>
        public bool DownloadNgrok { get; set; }

        /// <summary>
        /// Time in milliseconds to wait for the ngrok process to start <br/>
        /// <i>Defaults to 5 seconds</i>
        /// </summary>
        public int TimeoutMilliseconds { get; set; } = 2500;

        /// <summary>
        /// Determine if Agent Api requests should throw an exception when an error occurs <br/>
        /// <i>Defaults to true</i>
        /// </summary>
        public bool ThrowOnError { get; set; } = true;
    }
}
