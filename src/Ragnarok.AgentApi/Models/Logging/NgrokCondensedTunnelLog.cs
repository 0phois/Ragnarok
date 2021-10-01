using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Ragnarok.Test")]
namespace Ragnarok.AgentApi.Models
{
    internal class NgrokCondensedTunnelLog : NgrokCondensedLog
    {
        [JsonPropertyName("addr")]
        public string Address { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        public override string ToString()
        {
            var url = string.IsNullOrEmpty(Url) ? string.Empty : $"url={Url} ";
            var address = string.IsNullOrEmpty(Address) ? string.Empty : $"addr={Address} ";

            return base.ToString() + $"{address}{url}";
        }
    }
}
