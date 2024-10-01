namespace TaskingSystem.Application.CQRS.Commands
{
    public class CreateUserCommand
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid IdRole { get; set; }
        public Guid IdUserCreated { get; set; }
    }
}
