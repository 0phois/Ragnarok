using System;

namespace Ragnarok.AgentApi.Exceptions
{
    [Serializable]
    internal class NgrokProcessException : ApplicationException
    {
        public NgrokProcessException() : base("Ngrok process exited prematurely") { }

        public NgrokProcessException(string message) : base($"Ngrok process exited prematurely. {message}") { }
    }
}
