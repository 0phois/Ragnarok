using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi
{
    /// <summary>
    /// ngork.yml log level 
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum NgrokConfigLogLevel
    {
        /// <summary>
        /// log level not set
        /// </summary>
        none,
        /// <summary>
        /// Show debug logs
        /// </summary>
        debug,
        /// <summary>
        /// Show info logs
        /// </summary>
        info,
        /// <summary>
        /// Show warnings and errors only
        /// </summary>
        warn,
        /// <summary>
        /// Show errors
        /// </summary>
        error,
        /// <summary>
        /// Only critical errors are displayed
        /// </summary>
        crit
    }
}
