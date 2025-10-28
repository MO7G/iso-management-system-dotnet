using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db;

public class StandardSectionConfiguration : IEntityTypeConfiguration<StandardSection>
{
    public void Configure(EntityTypeBuilder<StandardSection> entity)
    {
        entity.ToTable("StandardSections");

        entity.HasKey(ss => ss.SectionID);

        entity.Property(ss => ss.StandardID)
            .IsRequired();

        entity.Property(ss => ss.ParentSectionID)
            .IsRequired(false);

        entity.Property(ss => ss.Number)
            .HasMaxLength(50)
            .IsRequired();

        entity.Property(ss => ss.Title)
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(ss => ss.OrderIndex)
            .HasDefaultValue(0);

        entity.Property(ss => ss.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        entity.Property(ss => ss.ModifiedAt)
            .HasDefaultValueSql("GETDATE()");

        // 🔹 One-to-Many: Standard → StandardSection
        entity.HasOne(ss => ss.Standard)
            .WithMany(s => s.Sections)
            .HasForeignKey(ss => ss.StandardID)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔹 Self-referencing: ParentSection → ChildSections
        entity.HasOne(ss => ss.ParentSection)
            .WithMany(ss => ss.ChildSections)
            .HasForeignKey(ss => ss.ParentSectionID)
            .OnDelete(DeleteBehavior.Restrict); // prevent cascading delete to children

        // 🔹 One-to-Many: StandardSection → StandardTemplate
        entity.HasMany(ss => ss.Templates)
            .WithOne(st => st.Section)
            .HasForeignKey(st => st.SectionID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}