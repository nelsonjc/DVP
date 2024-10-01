using System.ComponentModel.DataAnnotations.Schema;

namespace TaskingSystem.Domain.Entities
{
    [Table("RolePermission", Schema = SchemaName.SECURITY)]
    public class RolePermission
    {
        [Column("IdPermission", TypeName = "uniqueidentifier")]
        public Guid IdPermission { get; set; }

        [Column("IdRole", TypeName = "uniqueidentifier")]
        public Guid IdRole { get; set; }

        // Propiedades de navegación
        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
