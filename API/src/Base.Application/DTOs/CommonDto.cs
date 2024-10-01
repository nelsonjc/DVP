namespace TaskingSystem.Application.DTOs
{
    public class CommonDto
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid IdUserCreated { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public Guid? IdUserUpdated { get; set; }
        public string? UserUpdated { get; set; }
    }
}
