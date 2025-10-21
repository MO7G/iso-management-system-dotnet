using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("Users");

        entity.HasKey(u => u.UserID);

        entity.Property(u => u.FirstName)
              .HasMaxLength(100)
              .IsRequired();

        entity.Property(u => u.LastName) // schema: NOT NULL
              .HasMaxLength(100)
              .IsRequired();

        entity.Property(u => u.Email)
              .HasMaxLength(200)
              .IsRequired();

        entity.HasIndex(u => u.Email).IsUnique();

        entity.Property(u => u.PasswordHash)
              .HasMaxLength(500)
              .IsRequired();

        entity.Property(u => u.IsActive)
              .HasDefaultValue(true);

        entity.Property(u => u.CreatedAt)
              .HasDefaultValueSql("GETDATE()");

        entity.Property(u => u.ModifiedAt)
              .HasDefaultValueSql("GETDATE()");

        // Users <-> Roles (implicit many-to-many mapped to existing join table)
        entity.HasMany(u => u.Roles)
              .WithMany(r => r.Users)
              .UsingEntity<Dictionary<string, object>>(
                  "UserRoleAssignments", // existing table name
                  // Right navigation: join -> Role
                  j => j
                      .HasOne<Role>()
                      .WithMany()
                      .HasForeignKey("RoleID")
                      .HasConstraintName("FK_UserRoleAssignments_Roles_RoleID")
                      .OnDelete(DeleteBehavior.Restrict),
                  // Left navigation: join -> User
                  j => j
                      .HasOne<User>()
                      .WithMany()
                      .HasForeignKey("UserID")
                      .HasConstraintName("FK_UserRoleAssignments_Users_UserID")
                      .OnDelete(DeleteBehavior.Restrict),
                  // Configure the join table shape
                  j =>
                  {
                      j.ToTable("UserRoleAssignments");
                      j.HasKey("UserID", "RoleID");
                      // map timestamp columns if you still have them in DB (optional)
                      j.Property<DateTime>("CreatedAt").HasDefaultValueSql("GETDATE()");
                      j.Property<DateTime>("ModifiedAt").HasDefaultValueSql("GETDATE()");
                  });
    }
}
