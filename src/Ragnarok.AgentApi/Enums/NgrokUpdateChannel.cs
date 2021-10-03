namespace Ragnarok.AgentApi
{
    /// <summary>
    /// The update channel determines the stability of released builds to update to
    /// </summary>
    public enum NgrokUpdateChannel
    {
        /// <summary>
        /// To be used for all production deployments.
        /// </summary>
        Stable,
        /// <summary>
        /// Update to new beta builds when available
        /// </summary>
        Beta
    }
}
