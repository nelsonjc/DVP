using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Infrastructure.Persistencia.EntityConfigurations
{
    public class StatusSystemConfiguration : IEntityTypeConfiguration<StatusSystem>
    {
        public void Configure(EntityTypeBuilder<StatusSystem> builder)
        {
            builder.HasKey(x => x.Id).HasName("PK_StatusSystem");
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.Id).HasColumnName("IdStatusSystem");

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
