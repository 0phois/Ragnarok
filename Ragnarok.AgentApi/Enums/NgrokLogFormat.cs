namespace Ragnarok.AgentApi
{
    public enum NgrokLogFormat
    {
        /// <summary>
        /// Same as logfmt (custom colored human format if standard out is a TTY)
        /// </summary>
        term,
        /// <summary>
        /// Human and machine friendly key/value pairs
        /// </summary>
        logfmt,
        /// <summary>
        /// Newline-separated JSON objects
        /// </summary>
        json
    }
}
