using iso_management_system.Models;
using iso_management_system.Models.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db.JoinEntities
{
    public class ProjectAssignmentsConfiguration : IEntityTypeConfiguration<ProjectAssignments>
    {
        public void Configure(EntityTypeBuilder<ProjectAssignments> builder)
        {
            builder.ToTable("ProjectAssignments");

            builder.HasKey(pa => pa.AssignmentId);

            builder.Property(pa => pa.AssignedAt)
                .HasDefaultValueSql("GETDATE()");

            // -----------------------------
            // 🔹 ProjectAssignment → Project (Many-to-One)
            // -----------------------------
            builder.HasOne(pa => pa.Project)
                .WithMany(p => p.ProjectAssignments)
                .HasForeignKey(pa => pa.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 🔹 ProjectAssignment → User (Many-to-One)
            // -----------------------------
            builder.HasOne(pa => pa.User)
                .WithMany(u => u.ProjectAssignments)
                .HasForeignKey(pa => pa.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 🔹 ProjectAssignment → ProjectRole (One-to-Many)
            // -----------------------------
            builder.HasMany(pa => pa.ProjectRoles)
                .WithOne(pr => pr.ProjectAssignment)
                .HasForeignKey(pr => pr.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}