using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Infrastructure.Persistencia.EntityConfigurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(rp => new { rp.IdRole, rp.IdPermission });

            builder.HasOne(rp => rp.Role)
                .WithMany(r => r.RolPermissions)
                .HasForeignKey(rp => rp.IdRole)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_RolePermission_Role_IdRole");

            builder.HasOne(rp => rp.Permission)
                .WithMany()  
                .HasForeignKey(rp => rp.IdPermission)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_RolePermission_Permission_IdPermission");
        }
    }
}
