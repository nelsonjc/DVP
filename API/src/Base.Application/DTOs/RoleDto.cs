using System.ComponentModel.DataAnnotations.Schema;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Application.DTOs
{
    public class RoleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public IEnumerable<PermissionDto> RolPermissions { get; set; }
    }
}
