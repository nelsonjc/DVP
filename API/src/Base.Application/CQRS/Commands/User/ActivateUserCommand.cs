namespace TaskingSystem.Application.CQRS.Commands
{
    public class ActivateUserCommand
    {
        public Guid IdUser { get; set; }
        public Guid IdUserUpdated { get; set; }
    }
}
