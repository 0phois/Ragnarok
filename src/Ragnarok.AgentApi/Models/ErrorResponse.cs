using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Models
{
    internal class Details
    {
        [JsonPropertyName("err")]
        public string ErrorMessage { get; set; }
    }

    internal class ErrorResponse
    {
        [JsonPropertyName("error_code")]
        public int NgrokErrorCode { get; set; }

        [JsonPropertyName("status_code")]
        public int HttpStatusCode { get; set; }

        [JsonPropertyName("msg")]
        public string Message { get; set; }

        [JsonPropertyName("details")]
        public Details Details { get; set; }

        public override string ToString()
        {
            return $"Ngrok Api returned a status of {HttpStatusCode}. {Message} | Error Code: {NgrokErrorCode} - {Details.ErrorMessage}";
        }
    }
}
