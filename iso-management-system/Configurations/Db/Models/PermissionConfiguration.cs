using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.PermissionID);

        builder.Property(p => p.PermissionName)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(p => p.PermissionName).IsUnique();

        builder.Property(p => p.Description)
            .HasMaxLength(200);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.ModifiedAt)
            .HasDefaultValueSql("GETDATE()");
        
        
        // Many-to-many relationship with Role is configured in RoleConfiguration
        // via the "RolePermissionMappings" join table.


    }
}