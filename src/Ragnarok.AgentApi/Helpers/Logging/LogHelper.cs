using Microsoft.Extensions.Logging;
using Ragnarok.AgentApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

[assembly: InternalsVisibleTo("Ragnarok.Test")]
namespace Ragnarok.AgentApi.Helpers
{
    internal static class LogHelper
    {
        public static readonly IReadOnlyDictionary<char, char> QuotationMap = new ReadOnlyDictionary<char, char>(new Dictionary<char, char>(2)
        {
            ['"'] = '"',
            ['{'] = '}'
        });

        public static void Log(this ILogger logger, string message)
        {
            try
            {
                var log = message.StartsWith('{') ? ParseJsonLog(message) : ParseFormattedLog(message);

                if (log.Level != NgrokLoggerLogLevel.None)
                    logger.Log((LogLevel)log.Level, message: "=> {message}", log);
            }
            catch (Exception ex)
            {
                logger.LogInformation("=> {message}", message);
                Debug.WriteLine($"Error formatting log message: {ex}");
            }

        }

        public static NgrokCondensedLog ParseJsonLog(string message) => message switch
        {
            _ when message.Contains(@"""obj"":""csess""") => JsonSerializer.Deserialize<NgrokCondensedSesionLog>(message),
            _ when message.Contains(@"""obj"":""tunnels") => JsonSerializer.Deserialize<NgrokCondensedTunnelLog>(message),
            _ when message.Contains(@"""addr"":") => JsonSerializer.Deserialize<NgrokCondensedTunnelLog>(message),
            _ when message.Contains(@"""pg"":") => JsonSerializer.Deserialize<NgrokCondensedPageLog>(message),
            _ => JsonSerializer.Deserialize<NgrokCondensedLog>(message),
        };

        public static NgrokCondensedLog ParseFormattedLog(string message)
        {
            var currentPosition = 0;
            var rawMessage = message.AsSpan();
            var jsonBuilder = new StringBuilder("{");

            while (currentPosition < rawMessage.Length)
            {
                var key = GetLogKey(rawMessage, ref currentPosition);
                var value = GetLogValue(rawMessage, ref currentPosition);

                if (key.Length == 1 && key[0] == 't') value = value.Insert(value.Length - 2, ":");

                jsonBuilder.Append('"').Append(key).Append('"')
                           .Append(':')
                           .Append('"').Append(value).Append('"')
                           .Append(',');
            }

            jsonBuilder.Replace(',', '}', jsonBuilder.Length - 1, 1);

            return ParseJsonLog(jsonBuilder.ToString());
        }

        private static ReadOnlySpan<char> GetLogKey(ReadOnlySpan<char> span, ref int currentPosition)
        {
            span = span[currentPosition..];
            var index = span.IndexOf('=');

            if (index >= 0) currentPosition += index;

            return index == -1 ? null : span[..index];
        }

        private static string GetLogValue(ReadOnlySpan<char> span, ref int currentPosition)
        {
            bool isEscaping = false;
            span = span[++currentPosition..];

            if (span[0] == '"')
            {
                var quotes = new Stack<char>();
                var value = new StringBuilder();

                for (int position = 0; position < span.Length; position++)
                {
                    if (span[position] == '\\' && position + 1 < span.Length && span[position + 1] == '"') isEscaping = true;

                    if (isEscaping && span[position] == '"')
                        isEscaping = false;
                    else if (quotes.Count > 0 && span[position] == quotes.Peek())
                        quotes.Pop();
                    else if (QuotationMap.ContainsKey(span[position]))
                        quotes.Push(QuotationMap[span[position]]);

                    if (position != 0) value.Append(span[position]);

                    currentPosition += 1;

                    if (quotes.Count == 0) break;
                }

                currentPosition += 1;

                return value.ToString().TrimEnd('"');
            }
            else
            {
                var index = span.IndexOf(' ');

                if (index == -1) index = span.Length;

                currentPosition += index + 1;

                return index == -1 ? span.ToString() : span[..index].ToString();
            }
        }
    }
}
