using System.ComponentModel.DataAnnotations.Schema;

namespace TaskingSystem.Domain.Entities
{
    [Table("StatusSystem", Schema = "dbo")]
    public class StatusSystem : CommonEntity
    {
        [Column("Entity", TypeName = "varchar(100)")]
        public required string Entity { get; set; }

        [Column("Code", TypeName = "varchar(50)")]
        public required string Code { get; set; }

        [Column("Name", TypeName = "varchar(200)")]
        public required string Name { get; set; }

        [Column("Active", TypeName = "bit")]
        public required bool Active { get; set; }
    }
}