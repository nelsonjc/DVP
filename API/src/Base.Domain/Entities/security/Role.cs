using System.ComponentModel.DataAnnotations.Schema;

namespace TaskingSystem.Domain.Entities
{
    [Table("Role", Schema = SchemaName.SECURITY)]
    public class Role
    {
        [Column("IdRol", TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        [Column("Name", TypeName = "varchar(200)")]
        public string Name { get; set; }

        [Column("Active", TypeName = "bit")]
        public bool Active { get; set; }

        // Relación con RolePermission
        public virtual ICollection<RolePermission> RolPermissions { get; set; }
    }
}
