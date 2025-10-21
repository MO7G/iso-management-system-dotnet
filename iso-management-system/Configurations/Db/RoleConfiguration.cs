using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.RoleID);

        builder.Property(r => r.RoleName)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasIndex(r => r.RoleName).IsUnique();

        builder.Property(r => r.Description)
               .HasMaxLength(200);

        builder.Property(r => r.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        builder.Property(r => r.ModifiedAt)
               .HasDefaultValueSql("GETDATE()");

        // Roles <-> Permissions (implicit many-to-many mapped to existing join table)
        builder.HasMany(r => r.Permissions)
               .WithMany(p => p.Roles)
               .UsingEntity<Dictionary<string, object>>(
                   "RolePermissionMappings",
                   // Right navigation: join -> Permission
                   j => j
                       .HasOne<Permission>()
                       .WithMany()
                       .HasForeignKey("PermissionID")
                       .HasConstraintName("FK_RolePermissionMappings_Permissions_PermissionID")
                       .OnDelete(DeleteBehavior.Restrict),
                   // Left navigation: join -> Role
                   j => j
                       .HasOne<Role>()
                       .WithMany()
                       .HasForeignKey("RoleID")
                       .HasConstraintName("FK_RolePermissionMappings_Roles_RoleID")
                       .OnDelete(DeleteBehavior.Cascade),
                   j =>
                   {
                       j.ToTable("RolePermissionMappings");
                       j.HasKey("RoleID", "PermissionID");
                       j.Property<DateTime>("CreatedAt").HasDefaultValueSql("GETDATE()");
                       j.Property<DateTime>("ModifiedAt").HasDefaultValueSql("GETDATE()");
                   });
    }
}
