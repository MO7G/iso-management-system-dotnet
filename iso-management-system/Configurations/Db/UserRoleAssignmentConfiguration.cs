using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Configurations.Db
{
    public class UserRoleAssignmentConfiguration : IEntityTypeConfiguration<UserRoleAssignment>
    {
        public void Configure(EntityTypeBuilder<UserRoleAssignment> entity)
        {
            entity.ToTable("UserRoleAssignments");

            entity.HasKey(ura => new { ura.UserID, ura.RoleID });

            entity.HasOne(ura => ura.User)
                .WithMany(u => u.UserRoleAssignments)
                .HasForeignKey(ura => ura.UserID);

            entity.HasOne(ura => ura.Role)
                .WithMany(r => r.UserRoleAssignments)
                .HasForeignKey(ura => ura.RoleID);
        }
    }
}