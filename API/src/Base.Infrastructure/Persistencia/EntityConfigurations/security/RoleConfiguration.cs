using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Infrastructure.Persistencia.EntityConfigurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id).HasName("PK_Role");
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.Id).HasColumnName("IdRole");

            builder.HasMany(r => r.RolPermissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.IdRole)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Role_Permission");
        }
    }
}
