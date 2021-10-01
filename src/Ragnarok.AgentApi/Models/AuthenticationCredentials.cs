namespace Ragnarok.AgentApi.Models
{
    /// <summary>
    /// HTTP Basic authentication for tunnel
    /// </summary>
    public class AuthenticationCredentials
    {
        /// <summary>
        /// Username to enforce on tunneled requests
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password to enforce on tunneled requests
        /// </summary>
        public string Password { get; set; }

        public override string ToString()
        {
            return $"{Username}:{Password}";
        }
    }
}
