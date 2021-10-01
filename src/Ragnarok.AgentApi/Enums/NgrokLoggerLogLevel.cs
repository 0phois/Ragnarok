using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum NgrokLoggerLogLevel
    {
        None,
        Dbug,
        Info,
        Warn,
        Eror,
        Crit
    }
}
