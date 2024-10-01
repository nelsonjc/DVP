namespace TaskingSystem.Application.DTOs
{
    public class WorkItemDto : CommonDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Guid IdUser { get; set; }
        public string User { get; set; }
        public bool Active { get; set; }
        public Guid IdStatus { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public IEnumerable<WorkItemFlowDto> Flows { get; set; }
        public IEnumerable<WorkItemLogDto> Logs { get; set; }
    }

    public class WorkItemFlowDto
    {
        public Guid Id { get; set; }

        public Guid IdPreviousStatus { get; set; }
        public string PreviousStatus { get; set; }
        public Guid IdNewStatus { get; set; }
        public string NewStatus { get; set; }
        public string Observation { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid IdUserCreated { get; set; }
        public string UserCreated { get; set; }

    }

    public class WorkItemLogDto
    {
        public Guid Id { get; set; }
        public string TypeEvent { get; set; }
        public string LogEvent { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid IdUserCreated { get; set; }
        public string UserCreated { get; set; }
    }
}
