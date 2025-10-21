using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db
{
    public class DocumentStatusConfiguration : IEntityTypeConfiguration<DocumentStatus>
    {
        public void Configure(EntityTypeBuilder<DocumentStatus> builder)
        {
            builder.ToTable("DocumentStatuses");

            // Primary Key
            builder.HasKey(ds => ds.StatusID);

            // Properties
            builder.Property(ds => ds.StatusName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(ds => ds.Description)
                .HasMaxLength(200);

            builder.Property(ds => ds.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(ds => ds.ModifiedAt)
                .HasDefaultValueSql("GETDATE()");

            // -----------------------------
            // 🔹 DocumentStatus → ProjectDocuments (One-to-Many)
            // -----------------------------
            builder.HasMany(ds => ds.ProjectDocuments)
                .WithOne(pd => pd.Status)
                .HasForeignKey(pd => pd.StatusID)
                .OnDelete(DeleteBehavior.Restrict); // Usually restrict to avoid accidental deletion
        }
    }
}