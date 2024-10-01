using System.ComponentModel.DataAnnotations.Schema;

namespace TaskingSystem.Domain.Entities
{
    [Table("User", Schema = SchemaName.SECURITY)]
    public class User : CommonEntity
    {
        [Column("FullName", TypeName = "varchar(250)")]
        public string FullName { get; set; }

        [Column("UserName", TypeName = "varchar(50)")]
        public string UserName { get; set; }

        [Column("Password", TypeName = "varchar(500)")]
        public string Password { get; set; }

        [Column("Active", TypeName = "bit")]
        public bool Active { get; set; }

        [Column("IdRole", TypeName = "uniqueidentifier")]
        [ForeignKey("Role")]
        public Guid IdRole { get; set; }

        public Role Role { get; set; }

        public ICollection<WorkItem> Tasks { get; set; }
    }
}
