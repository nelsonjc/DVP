namespace TaskingSystem.Application.CQRS.Commands
{
    public class CreateWorkItemCommand
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdUserCreated { get; set; }
    }
}
