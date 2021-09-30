using System.ComponentModel;

namespace Ragnarok.HostedService.Models
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class AuthToken
    {
        public string RawValue { get; }

        public AuthToken(string token) => RawValue = token;
    }
}
