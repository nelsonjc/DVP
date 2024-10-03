namespace TaskingSystem.Application.CQRS.Commands
{
    public class ChangePasswordCommand
    {
        public Guid IdUser { get; set; }
        public Guid IdUserUpdated { get; set; }
        public string Password { get; set; }
    }
}
