namespace TaskingSystem.Application.CQRS.Commands
{
    public class ChangeStatusWorkItemCommand
    {
        public Guid Id { get; set; }
        public Guid IdStatus { get; set; }
        public Guid IdUserUpdated { get; set; }
        public string Observation { get; set; }
    }
}
