using System;

namespace Ragnarok.AgentApi.Exceptions
{
    [Serializable]
    internal class DuplicateProcessException : ApplicationException
    {
        public DuplicateProcessException() : base("Another instance of ngrok is currently active. The requested action cannot be executed.") { }

        public DuplicateProcessException(string message) : base(message) { }
    }
}
