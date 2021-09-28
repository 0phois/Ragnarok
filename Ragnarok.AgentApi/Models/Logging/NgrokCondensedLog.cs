using Ragnarok.AgentApi.Helpers;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Ragnarok.Test")]
namespace Ragnarok.AgentApi.Models
{
    internal class NgrokCondensedLog
    {
        [JsonPropertyName("t")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonPropertyName("lvl")]
        public NgrokLoggerLogLevel Level { get; set; }
        
        [JsonPropertyName("msg")]
        public string Message { get; set; }

        [JsonPropertyName("obj")]
        public string Object { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public int? StatusCode { get; set; }

        [JsonPropertyName("err")]
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            var id = string.IsNullOrEmpty(Id) ? string.Empty : $"id={Id} ";
            var obj = string.IsNullOrEmpty(Object) ? string.Empty : $"obj={Object} ";
            var name = string.IsNullOrEmpty(Name) ? string.Empty : $"name={Name} ";
            var status = StatusCode.HasValue ? $"status={StatusCode}: {StatusHelper.StatusMap[StatusCode.Value]} " : string.Empty;
            var error = string.IsNullOrEmpty(ErrorMessage) || ErrorMessage.Equals("nil") || ErrorMessage.Equals("\u003cnil\u003e")
                            ? string.Empty 
                            : $"| err={ErrorMessage} ";

            return $"[{Timestamp:MMM-dd|HH:mm:ss}] {Message, -50} {obj}{name}{id}{status}{error}";
        }
    }
}
