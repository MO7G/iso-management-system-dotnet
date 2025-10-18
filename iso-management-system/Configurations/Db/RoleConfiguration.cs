using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.RoleID);

            builder.Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Description)
                .HasMaxLength(255);

            // 🔹 One Role -> many RolePermissionMappings
            builder.HasMany(r => r.RolePermissionMappings)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleID)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 One Role -> many UserRoleAssignments
            builder.HasMany(r => r.UserRoleAssignments)
                .WithOne(ura => ura.Role)
                .HasForeignKey(ura => ura.RoleID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}