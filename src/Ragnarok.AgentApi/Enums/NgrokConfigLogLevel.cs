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
        None,
        /// <summary>
        /// Show debug logs
        /// </summary>
        Debug,
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
        Error,
        /// <summary>
        /// Only critical errors are displayed
        /// </summary>
        Crit
    }
}
