using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum NgrokConfigLogLevel
    {
        None,
        Debug,
        Info,
        Warn,
        Error,
        Crit
    }
}
