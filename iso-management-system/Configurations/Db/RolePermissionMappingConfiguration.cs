using iso_management_system.Models.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Data.Configurations
{
    public class RolePermissionMappingConfiguration : IEntityTypeConfiguration<RolePermissionMapping>
    {
        public void Configure(EntityTypeBuilder<RolePermissionMapping> builder)
        {
            builder.ToTable("RolePermissionMappings");

            builder.HasKey(rp => new { rp.RoleID, rp.PermissionID });

            
            // we don't need to force restriction for cascade delete here
            builder.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissionMappings)
                .HasForeignKey(rp => rp.RoleID);

            builder.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissionMappings)
                .HasForeignKey(rp => rp.PermissionID)
                .OnDelete(DeleteBehavior.Restrict); // <-- prevents cascade delete
        }
    }
}