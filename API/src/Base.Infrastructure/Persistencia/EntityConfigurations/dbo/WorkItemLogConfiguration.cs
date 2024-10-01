using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Infrastructure.Persistencia.EntityConfigurations
{
    public class WorkItemLogConfiguration : IEntityTypeConfiguration<WorkItemLog>
    {
        public void Configure(EntityTypeBuilder<WorkItemLog> builder)
        {
            builder.HasKey(x => x.Id).HasName("PK_TaskLog");
            builder.Property(x => x.Id).HasColumnName("IdTaskLog");

            builder.HasOne(rp => rp.WorkItem)
                .WithMany()
                .HasForeignKey(rp => rp.IdWorkItem)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(rp => rp.UserCreated)
              .WithMany()
              .HasForeignKey(rp => rp.IdUserCreated)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
