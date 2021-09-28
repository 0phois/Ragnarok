using System;

namespace Ragnarok.AgentApi.Exceptions
{
    [Serializable]
    internal class NotInitializedException : ApplicationException
    {
        public NotInitializedException() : base("The RagnarokClient must be initialized before performing this action") { }

        public NotInitializedException(string message) : base(message) { }
    }
}
