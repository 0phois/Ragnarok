using System;

namespace Ragnarok.AgentApi.Exceptions
{
    [Serializable]
    internal class NgrokApiException : ApplicationException
    {
        public NgrokApiException(string message) : base(message) { }
    }
}