using System.ComponentModel.DataAnnotations.Schema;

namespace TaskingSystem.Domain.Entities
{
    [Table("Task", Schema = "dbo")]
    public class WorkItem : CommonEntity
    {
        public WorkItem()
        {
            Flows = new HashSet<WorkItemFlow>();
            Logs = new HashSet<WorkItemLog>();
        }

        [Column("Title", TypeName = "varchar(250)")]
        public required string Title { get; set; }

        [Column("Description", TypeName = "varchar(max)")]
        public required string Description { get; set; }

        [Column("DueDate", TypeName = "datetime")]
        public required DateTime DueDate { get; set; }

        [Column("IdUser", TypeName = "uniqueidentifier")]
        [ForeignKey("User")]
        public required Guid IdUser { get; set; }

        [Column("Active", TypeName = "bit")]
        public required bool Active { get; set; }

        [Column("IdStatus", TypeName = "uniqueidentifier")]
        [ForeignKey("Status")]
        public required Guid IdStatus { get; set; }

        public required virtual StatusSystem Status { get; set; }
        public required virtual User User { get; set; }
        public required virtual ICollection<WorkItemFlow> Flows { get; set; }
        public required virtual ICollection<WorkItemLog> Logs { get; set; } 
    }
}
