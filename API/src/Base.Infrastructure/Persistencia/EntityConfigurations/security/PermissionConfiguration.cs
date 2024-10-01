using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Infrastructure.Persistencia.EntityConfigurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(x => x.Id).HasName("PK_Permission");
            builder.Property(x => x.Id).HasColumnName("IdPermission");

            builder.HasOne(rp => rp.UserCreated)
                .WithMany()
                .HasForeignKey(rp => rp.IdUserCreated)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(rp => rp.UserUpdated)
                .WithMany()
                .HasForeignKey(rp => rp.IdUserUpdated)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
