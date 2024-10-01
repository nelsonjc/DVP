namespace TaskingSystem.Domain.Entities
{
    public class CommonEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(-5);
        public Guid IdUserCreated { get; set; }
        public User UserCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public Guid? IdUserUpdated { get; set; }
        public User? UserUpdated { get; set; }
    }
}
