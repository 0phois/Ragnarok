namespace Ragnarok.AgentApi
{
    /// <summary>
    /// Possible options for enabling/disabling the console ui in the ngrok.yml file
    /// </summary>
    public enum ConsoleOptions
    {
        /// <summary>
        /// Enable the UI only if standard out is a TTY (not a file or pipe)
        /// </summary>
        iftty,
        /// <summary>
        /// Enable the console UI
        /// </summary>
        @true,
        /// <summary>
        /// Disable the console UI
        /// </summary>
        @false
    }
}
