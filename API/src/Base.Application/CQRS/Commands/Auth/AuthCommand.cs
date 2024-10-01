namespace TaskingSystem.Application.CQRS.Commands
{
    /// <summary>
    /// Represents a command for user authentication containing the necessary credentials.
    /// </summary>
    public class AuthCommand
    {
        /// <summary>
        /// Gets or sets the username of the user attempting to authenticate.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password of the user attempting to authenticate.
        /// </summary>
        public string Password { get; set; }
    }
}
