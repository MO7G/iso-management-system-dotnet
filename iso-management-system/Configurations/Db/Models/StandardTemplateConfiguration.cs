using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db
{
    public class StandardTemplateConfiguration : IEntityTypeConfiguration<StandardTemplate>
    {
        public void Configure(EntityTypeBuilder<StandardTemplate> entity)
        {
            entity.ToTable("StandardTemplates");

            entity.HasKey(st => st.TemplateID);

            entity.Property(st => st.SectionID)
                .IsRequired();

            entity.Property(st => st.FileID)
                .IsRequired();

            entity.Property(st => st.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            entity.Property(st => st.ModifiedAt)
                .HasDefaultValueSql("GETDATE()");

            // 🔹 One-to-Many: StandardSection → StandardTemplate
            entity.HasOne(st => st.Section)
                .WithMany(s => s.Templates)
                .HasForeignKey(st => st.SectionID)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 One-to-Many: FileStorage → StandardTemplate
            entity.HasOne(st => st.File)
                .WithMany(f => f.StandardTemplates)
                .HasForeignKey(st => st.FileID)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 One-to-Many: StandardTemplate → ProjectDocuments
            entity.HasMany(st => st.ProjectDocuments)
                .WithOne(pd => pd.Template)
                .HasForeignKey(pd => pd.TemplateID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}