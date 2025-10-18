using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using iso_management_system.Models;

namespace iso_management_system.Configurations.Db
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users");

            entity.HasKey(u => u.UserID);

            entity.Property(u => u.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(u => u.LastName)
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .HasMaxLength(200)
                .IsRequired();

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .HasMaxLength(500)
                .IsRequired();

            // Relationships
            entity.HasMany(u => u.UserRoleAssignments)
                .WithOne(ura => ura.User)
                .HasForeignKey(ura => ura.UserID);
        }
    }
}