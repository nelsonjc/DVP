using System.ComponentModel.DataAnnotations.Schema;

namespace TaskingSystem.Domain.Entities
{
    [Table("WorkItemLog", Schema = "dbo")]
    public class WorkItemLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("IdWorkItem", TypeName = "uniqueidentifier")]
        [ForeignKey("WorkItem")]
        public Guid IdWorkItem { get; set; }

        [Column("TypeEvent", TypeName = "varchar(50)")]
        public string TypeEvent { get; set; }

        [Column("LogEvent", TypeName = "varchar(1000)")]
        public string LogEvent { get; set; }

        [Column("DateCreated", TypeName = "datetime")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(-5);

        [Column("IdUserCreated", TypeName = "uniqueidentifier")]
        [ForeignKey("UserCreated")]
        public Guid IdUserCreated { get; set; }

        public virtual WorkItem WorkItem { get; set; }
        public virtual User UserCreated { get; set; }
    }
}
