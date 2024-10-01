using System.ComponentModel.DataAnnotations.Schema;

namespace TaskingSystem.Domain.Entities
{
    [Table("WorkItemFlow", Schema = "dbo")]
    public class WorkItemFlow
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("IdWorkItem", TypeName = "uniqueidentifier")]
        [ForeignKey("WorkItem")]
        public Guid IdWorkItem { get; set; }

        [Column("IdPreviousStatus", TypeName = "uniqueidentifier")]
        [ForeignKey("PreviousStatus")]
        public Guid IdPreviousStatus { get; set; }

        [Column("IdNewStatus", TypeName = "uniqueidentifier")]
        [ForeignKey("NewStatus")]
        public Guid IdNewStatus { get; set; }

        [Column("Observation", TypeName = "varchar(1000)")]
        public string Observation { get; set; }

        [Column("DateCreated", TypeName = "datetime")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(-5);

        [Column("IdUserCreated", TypeName = "uniqueidentifier")]
        [ForeignKey("UserCreated")]
        public Guid IdUserCreated { get; set; }

        public virtual WorkItem WorkItem { get; set; }
        public virtual StatusSystem PreviousStatus { get; set; }
        public virtual StatusSystem NewStatus { get; set; }
        public virtual User UserCreated { get; set; }
    }
}
