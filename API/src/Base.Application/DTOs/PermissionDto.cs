namespace TaskingSystem.Application.DTOs
{
    public class PermissionDto : CommonDto
    {
        public string Entity { get; set; }
        public int IdActionType { get; set; }
        public string ActionType { get; set; }
    }
}
