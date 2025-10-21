using iso_management_system.Models.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db.JoinEntities
{
    public class ProjectDocumentsConfiguration : IEntityTypeConfiguration<ProjectDocuments>
    {
        public void Configure(EntityTypeBuilder<ProjectDocuments> builder)
        {
            builder.ToTable("ProjectDocuments");

            builder.HasKey(pd => pd.ProjectDocumentID);

            // Metadata
            builder.Property(pd => pd.VersionNumber)
                   .HasDefaultValue(1);

            builder.Property(pd => pd.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(pd => pd.ModifiedAt)
                   .HasDefaultValueSql("GETDATE()");

            // -----------------------------
            // 🔹 ProjectDocument → Project (Many-to-One)
            // -----------------------------
            builder.HasOne(pd => pd.Project)
                   .WithMany(p => p.ProjectDocuments)
                   .HasForeignKey(pd => pd.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 🔹 ProjectDocument → StandardTemplate (Many-to-One)
            // -----------------------------
            builder.HasOne(pd => pd.Template)
                   .WithMany(st => st.ProjectDocuments)
                   .HasForeignKey(pd => pd.TemplateID)
                   .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 🔹 ProjectDocument → FileStorage (Many-to-One)
            // -----------------------------
            builder.HasOne(pd => pd.File)
                   .WithMany(f => f.ProjectDocuments)
                   .HasForeignKey(pd => pd.FileID)
                   .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 🔹 ProjectDocument → DocumentStatus (Many-to-One)
            // -----------------------------
            builder.HasOne(pd => pd.Status)
                   .WithMany(ds => ds.ProjectDocuments)
                   .HasForeignKey(pd => pd.StatusID)
                   .OnDelete(DeleteBehavior.Restrict);

            // -----------------------------
            // 🔹 ProjectDocument → User (LastModifiedBy, optional)
            // -----------------------------
            builder.HasOne(pd => pd.LastModifiedUser)
                   .WithMany()
                   .HasForeignKey(pd => pd.LastModifiedBy)
                   .OnDelete(DeleteBehavior.SetNull);

            // -----------------------------
            // 🔹 ProjectDocument → DocumentRevisions (One-to-Many)
            // -----------------------------
            builder.HasMany(pd => pd.DocumentRevisions)
                   .WithOne(dr => dr.ProjectDocument)
                   .HasForeignKey(dr => dr.ProjectDocumentID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
