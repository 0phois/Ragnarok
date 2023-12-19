using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi
{
    /// <summary>
    /// Create an HTTP or HTTPS endpoint (or both)
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Scheme
    {
        /// <summary>
        /// Default
        /// </summary>
        none,
        /// <summary>
        /// Opens endpoints for both HTTP and HTTPS traffic <br/>
        /// <i>Default behavior</i>
        /// </summary>
        both,
        /// <summary>
        /// Only listen on an HTTP tunnel endpoint
        /// </summary>
        http,
        /// <summary>
        /// Only listen on an HTTPS tunnel endpoint
        /// </summary>
        https
    }
}
