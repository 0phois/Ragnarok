using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Ragnarok.Test")]
namespace Ragnarok.AgentApi.Models
{
    internal class NgrokCondensedPageLog : NgrokCondensedLog
    {
        [JsonPropertyName("pg")]
        public string Page { get; set; }

        [JsonPropertyName("dur")]
        public string Duration { get; set; }


        public override string ToString()
        {
            var duration = string.IsNullOrEmpty(Duration) ? string.Empty : $"| dur={Duration} ";

            return base.ToString() + $"page={Page} {duration}";
        }
    }
}
