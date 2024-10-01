namespace TaskingSystem.Application.DTOs
{
    public class UserDto : CommonDto
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public bool Active { get; set; }
        public Guid IdRole { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<PermissionDto> Permissions { get; set; }
        public int TaskPending { get; set; }
        public int TaskInProcess { get; set; }
        public int TaskCompleted { get; set; }
        public int TaskDue { get; set; }
    }
}
