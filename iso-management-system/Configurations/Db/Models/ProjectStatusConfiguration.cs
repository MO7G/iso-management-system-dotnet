using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db
{
    public class ProjectStatusConfiguration : IEntityTypeConfiguration<ProjectStatus>
    {
        public void Configure(EntityTypeBuilder<ProjectStatus> builder)
        {
            builder.ToTable("ProjectStatus");

            builder.HasKey(ps => ps.StatusId);

            builder.Property(ps => ps.StatusName)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(ps => ps.StatusName).IsUnique();

            builder.Property(ps => ps.Description)
                .HasMaxLength(200);

            builder.Property(ps => ps.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(ps => ps.ModifiedAt)
                .HasDefaultValueSql("GETDATE()");

            // One-to-Many: ProjectStatus → Projects
            builder.HasMany(ps => ps.Projects)
                .WithOne(p => p.ProjectStatus)
                .HasForeignKey(p => p.ProjectStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}