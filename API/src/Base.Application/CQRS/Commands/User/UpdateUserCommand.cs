namespace TaskingSystem.Application.CQRS.Commands
{
    public class UpdateUserCommand
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public bool Active { get; set; }
        public Guid IdRole { get; set; }
        public Guid IdUserUpdated { get; set; }
    }
}
