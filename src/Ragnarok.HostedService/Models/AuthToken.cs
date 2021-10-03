using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace Ragnarok.HostedService.Models
{
    /// <summary>
    /// Used the store the ngrok authtoken in the <see cref="IServiceCollection"/>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class AuthToken
    {
        /// <summary>
        /// String value of the token
        /// </summary>
        public string RawValue { get; }

        /// <param name="token">ngrok authtoken</param>
        public AuthToken(string token) => RawValue = token;
    }
}
