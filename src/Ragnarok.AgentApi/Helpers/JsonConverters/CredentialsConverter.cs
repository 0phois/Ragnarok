using Ragnarok.AgentApi.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ragnarok.AgentApi.Helpers
{
    internal class CredentialsConverter : JsonConverter<AuthenticationCredentials[]>
    {
        public override AuthenticationCredentials[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var data = jsonDoc.RootElement.GetRawText().Split(':', 2);

            if (data.Length != 2)
                throw new FormatException("The credentials provided does not match the expected format 'username:password'");

            return [new AuthenticationCredentials() { Username = data[0], Password = data[1] }];
        }

        public override void Write(Utf8JsonWriter writer, AuthenticationCredentials[] value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var credential in value)
                writer.WriteStringValue($"{credential.Username}:{credential.Password}");

            writer.WriteEndArray();
        }
    }
}
