using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi
{
    /// <summary>
    /// ngrok client log output
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum NgrokLoggerLogLevel
    {
        /// <summary>
        /// Log level not set
        /// </summary>
        None,
        /// <summary>
        /// Show debug logs
        /// </summary>
        Dbug,
        /// <summary>
        /// Show info logs
        /// </summary>
        Info,
        /// <summary>
        /// Show warnings and errors only
        /// </summary>
        Warn,
        /// <summary>
        /// Show errors
        /// </summary>
        Eror,
        /// <summary>
        /// Only critical errors are displayed
        /// </summary>
        Crit
    }
}
