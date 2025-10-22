using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");

            builder.HasKey(p => p.ProjectId);

            builder.Property(p => p.StartDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.CompletionDate);

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.ModifiedAt)
                   .HasDefaultValueSql("GETDATE()");

            // -----------------------------
            // 🔹 Project → Customer (One-to-Many)
            // -----------------------------
            builder.HasOne(p => p.Customer)
                   .WithMany(c => c.Projects)
                   .HasForeignKey(p => p.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // -----------------------------
            // 🔹 Project → ProjectStatus (One-to-Many)
            // -----------------------------
            builder.HasOne(p => p.ProjectStatus)
                   .WithMany(ps => ps.Projects)
                   .HasForeignKey(p => p.StatusID) // match DB
                   .OnDelete(DeleteBehavior.Restrict);

            // -----------------------------
            // 🔹 Project → Standard (One-to-Many)
            // -----------------------------
            builder.HasOne(p => p.Standard)
                   .WithMany(s => s.Projects)
                   .HasForeignKey(p => p.StandardID)
                   .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 🔹 Project → ProjectAssignments (One-to-Many)
            // -----------------------------
            builder.HasMany(p => p.ProjectAssignments)
                   .WithOne(pa => pa.Project)
                   .HasForeignKey(pa => pa.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 🔹 Project → ProjectDocuments (One-to-Many)
            // -----------------------------
            builder.HasMany(p => p.ProjectDocuments)
                   .WithOne(pd => pd.Project)
                   .HasForeignKey(pd => pd.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
