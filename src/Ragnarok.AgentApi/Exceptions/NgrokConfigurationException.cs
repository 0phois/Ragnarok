using System;

namespace Ragnarok.AgentApi.Exceptions
{
    [Serializable]
    internal class NgrokConfigurationException : ApplicationException
    {
        public NgrokConfigurationException() : base("Review ngrok.yml configurations") { }

        public NgrokConfigurationException(string message) : base($"Review ngrok.yml configurations. {message}") { }
    }
}
