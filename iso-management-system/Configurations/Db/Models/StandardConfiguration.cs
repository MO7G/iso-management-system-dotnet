using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db;

public class StandardConfiguration : IEntityTypeConfiguration<Standard>
{
    public void Configure(EntityTypeBuilder<Standard> entity)
    {
        entity.ToTable("Standards");

        entity.HasKey(s => s.StandardID);

        entity.Property(s => s.Name)
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(s => s.Version)
            .HasMaxLength(50)
            .IsRequired(false);

        entity.Property(s => s.PublishedDate)
            .IsRequired(false);

        entity.Property(s => s.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        entity.Property(s => s.ModifiedAt)
            .HasDefaultValueSql("GETDATE()");

        // 🔹 One-to-Many: Standard → StandardSections
        entity.HasMany(s => s.Sections)
            .WithOne(ss => ss.Standard)
            .HasForeignKey(ss => ss.StandardID)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔹 One-to-Many: Standard → Projects
        entity.HasMany(s => s.Projects)
            .WithOne(p => p.Standard)
            .HasForeignKey(p => p.StandardID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}