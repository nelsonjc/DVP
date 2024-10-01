using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Infrastructure.Persistencia.EntityConfigurations
{
    public class WorkItemFlowConfiguration : IEntityTypeConfiguration<WorkItemFlow>
    {
        public void Configure(EntityTypeBuilder<WorkItemFlow> builder)
        {
            builder.HasKey(x => x.Id).HasName("PK_TaskFlow");
            builder.Property(x => x.Id).HasColumnName("IdTaskFlow");

            builder.HasOne(rp => rp.WorkItem)
                .WithMany()
                .HasForeignKey(rp => rp.IdWorkItem)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(rp => rp.PreviousStatus)
               .WithMany()
               .HasForeignKey(rp => rp.IdPreviousStatus)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(rp => rp.NewStatus)
             .WithMany()
             .HasForeignKey(rp => rp.IdNewStatus)
             .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(rp => rp.UserCreated)
              .WithMany()
              .HasForeignKey(rp => rp.IdUserCreated)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
