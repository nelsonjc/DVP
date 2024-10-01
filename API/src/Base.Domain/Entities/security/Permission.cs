using System.ComponentModel.DataAnnotations.Schema;

namespace TaskingSystem.Domain.Entities
{
    [Table("Permission", Schema = SchemaName.SECURITY)]
    public class Permission : CommonEntity
    {
        public required string Entity { get; set; }

        public required ActionTypeEnum ActionType { get; set; } 
    }

    public enum ActionTypeEnum
    {
        Create = 1,
        Read = 2,
        Update = 3,
        Delete = 4,
        Asigne = 5,
        ChangeStatus = 6,
    }
}
