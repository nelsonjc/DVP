using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Infrastructure.Persistencia.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id).HasName("PK_User");
            builder.HasIndex(x => x.UserName).IsUnique();
            builder.Property(x => x.Id).HasColumnName("IdUser");

            builder.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.IdRole)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_User_Role");

            builder.HasMany(x => x.Tasks)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.IdUser)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_User_Task");

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
