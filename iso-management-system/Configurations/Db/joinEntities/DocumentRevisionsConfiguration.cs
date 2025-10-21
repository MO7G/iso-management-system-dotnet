using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db.JoinEntities
{
    public class DocumentRevisionConfiguration : IEntityTypeConfiguration<DocumentRevision>
    {
        public void Configure(EntityTypeBuilder<DocumentRevision> builder)
        {
            builder.ToTable("DocumentRevisions");

            builder.HasKey(dr => dr.RevisionID);

            builder.Property(dr => dr.ProjectDocumentID)
                   .IsRequired();

            builder.Property(dr => dr.FileID)
                   .IsRequired();

            builder.Property(dr => dr.VersionNumber)
                   .IsRequired();

            builder.Property(dr => dr.ModifiedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(dr => dr.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(dr => dr.ChangeNote)
                   .HasMaxLength(500);

            // -----------------------------
            // 🔹 DocumentRevision → ProjectDocuments (Many-to-One)
            // -----------------------------
            builder.HasOne(dr => dr.ProjectDocument)
                   .WithMany(pd => pd.DocumentRevisions)
                   .HasForeignKey(dr => dr.ProjectDocumentID)
                   .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 🔹 DocumentRevision → FileStorage (Many-to-One)
            // -----------------------------
            builder.HasOne(dr => dr.File)
                   .WithMany(f => f.DocumentRevisions)
                   .HasForeignKey(dr => dr.FileID)
                   .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 🔹 DocumentRevision → User (ModifiedBy) (Many-to-One, optional)
            // -----------------------------
            builder.HasOne(dr => dr.ModifiedByUser)
                   .WithMany(u => u.DocumentRevisions)
                   .HasForeignKey(dr => dr.ModifiedByUserID)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
