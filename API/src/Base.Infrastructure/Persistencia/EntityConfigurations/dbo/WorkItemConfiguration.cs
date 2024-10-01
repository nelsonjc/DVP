using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Infrastructure.Persistencia.EntityConfigurations
{
    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> builder)
        {
            builder.HasKey(x => x.Id).HasName("PK_Task");
            builder.Property(x => x.Id).HasColumnName("IdTask");

            builder.HasOne(rp => rp.Status)
                .WithMany()
                .HasForeignKey(rp => rp.IdStatus)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(rp => rp.User)
                .WithMany(x => x.Tasks)
                .HasForeignKey(rp => rp.IdUser)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasOne(rp => rp.UserCreated)
                .WithMany()
                .HasForeignKey(rp => rp.IdUserCreated)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(rp => rp.UserUpdated)
                .WithMany()
                .HasForeignKey(rp => rp.IdUserUpdated)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Flows)
                .WithOne(x => x.WorkItem)
                .HasForeignKey(x => x.IdWorkItem)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Flow_Task");

            builder.HasMany(x => x.Logs)
                .WithOne(x => x.WorkItem)
                .HasForeignKey(x => x.IdWorkItem)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Log_Task");
        }
    }
}
