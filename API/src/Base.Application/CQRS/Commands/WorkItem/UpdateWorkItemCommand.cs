namespace TaskingSystem.Application.CQRS.Commands
{
    public class UpdateWorkItemCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdStatus { get; set; }
        public Guid IdUserUpdated { get; set; }
        public string Observation { get; set; }
    }
}
