using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi
{
    /// <summary>
    /// Reference: <see href="https://ngrok.com/docs#http-bind-tls">https://ngrok.com/docs#http-bind-tls</see>
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BindTLS
    {
        /// <summary>
        /// Opens endpoints for both HTTP and HTTPS traffic <br/>
        /// <i>Default behavior</i>
        /// </summary>
        Both,
        /// <summary>
        /// Only listen on an HTTP tunnel endpoint
        /// </summary>
        False,
        /// <summary>
        /// Only listen on an HTTPS tunnel endpoint
        /// </summary>
        True
    }
}
