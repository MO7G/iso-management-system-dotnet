using iso_management_system.Models;
using iso_management_system.Models.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace iso_management_system.Configurations.Db.JoinEntities
{
    public class ProjectRolesConfiguration : IEntityTypeConfiguration<ProjectRoles>
    {
        public void Configure(EntityTypeBuilder<ProjectRoles> builder)
        {
            builder.ToTable("ProjectRoles");

            // Composite primary key
            builder.HasKey(pr => new { pr.AssignmentId, pr.RoleId });

            // Metadata columns
            builder.Property(pr => pr.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(pr => pr.ModifiedAt)
                .HasDefaultValueSql("GETDATE()");

            // -----------------------------
            // 🔹 ProjectRole → ProjectAssignment (Many-to-One)
            // -----------------------------
            builder.HasOne(pr => pr.ProjectAssignment)
                .WithMany(pa => pa.ProjectRoles)
                .HasForeignKey(pr => pr.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 🔹 ProjectRole → Role (Many-to-One)
            // -----------------------------
            builder.HasOne(pr => pr.Role)
                .WithMany(r => r.ProjectRoles)
                .HasForeignKey(pr => pr.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}