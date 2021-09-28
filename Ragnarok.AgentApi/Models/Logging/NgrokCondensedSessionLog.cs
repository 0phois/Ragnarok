using Ragnarok.AgentApi.Helpers;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Ragnarok.Test")]
namespace Ragnarok.AgentApi.Models
{
    internal class NgrokCondensedSesionLog : NgrokCondensedLog
    {
        [JsonPropertyName("clientid")]
        public string ClientId { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [JsonPropertyName("sid")]
        public string Sid { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [JsonPropertyName("reqtype")]
        public string RequestType { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [JsonPropertyName("req")]
        public string Request { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [JsonPropertyName("resp")]
        public string Response { get; set; }

        public override string ToString()
        {
            var clientId = string.IsNullOrEmpty(ClientId) ? string.Empty : $"clientId={ClientId} ";
            var sid = string.IsNullOrEmpty(Sid) ? string.Empty : $"sid={Sid} ";
            var requestType = string.IsNullOrEmpty(RequestType) ? string.Empty : $"reqType={RequestType} ";
            var request = string.IsNullOrEmpty(Request) ? string.Empty : $"req={Request} ";
            var response = string.IsNullOrEmpty(Response) ? string.Empty : $"resp={Response} ";

            return base.ToString() + $"{clientId}{sid}{requestType}{request}{response}";
        }
    }
}
