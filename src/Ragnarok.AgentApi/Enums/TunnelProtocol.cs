using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TunnelProtocol
    {
        HTTP,
        HTTPS,
        TCP,
        TLS
    }
}
