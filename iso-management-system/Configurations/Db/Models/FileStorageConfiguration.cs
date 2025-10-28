using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db;

public class FileStorageConfiguration : IEntityTypeConfiguration<FileStorage>
{
    public void Configure(EntityTypeBuilder<FileStorage> builder)
    {
        builder.ToTable("FileStorage");

        // Primary Key
        builder.HasKey(f => f.FileID);

        // Properties
        builder.Property(f => f.FileName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(f => f.FilePath)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(f => f.FileSize);

        builder.Property(f => f.UploadedAt)
            .HasDefaultValueSql("GETDATE()");

        // -----------------------------
        // 🔹 FileStorage → User (Many-to-One)
        // -----------------------------
        builder.HasOne(f => f.UploadedByUser)
            .WithMany(u => u.UploadedFiles)
            .HasForeignKey(f => f.UploadedByUserID)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        // -----------------------------
        // 🔹 FileStorage → Customer (Many-to-One)
        // -----------------------------
        builder.HasOne(f => f.UploadedByCustomer)
            .WithMany(c => c.UploadedFiles)
            .HasForeignKey(f => f.UploadedByCustomerID)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        // -----------------------------
        // 🔹 FileStorage → DocumentRevisions (One-to-Many)
        // -----------------------------
        builder.HasMany(f => f.DocumentRevisions)
            .WithOne(dr => dr.File)
            .HasForeignKey(dr => dr.FileID)
            .OnDelete(DeleteBehavior.Cascade);

        // -----------------------------
        // 🔹 FileStorage → ProjectDocuments (One-to-Many)
        // -----------------------------
        builder.HasMany(f => f.ProjectDocuments)
            .WithOne(pd => pd.File)
            .HasForeignKey(pd => pd.FileID)
            .OnDelete(DeleteBehavior.Cascade);

        // -----------------------------
        // 🔹 FileStorage → StandardTemplates (One-to-Many)
        // -----------------------------
        builder.HasMany(f => f.StandardTemplates)
            .WithOne(st => st.File)
            .HasForeignKey(st => st.FileID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}