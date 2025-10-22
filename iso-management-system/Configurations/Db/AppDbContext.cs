using Microsoft.EntityFrameworkCore;
using iso_management_system.Models;
using iso_management_system.Configurations.Db.JoinEntities;
using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Configurations.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // -----------------------------
        // DbSets for main entities
        // -----------------------------
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectStatus> ProjectStatuses { get; set; }
        public DbSet<DocumentStatus> DocumentStatuses { get; set; }
        public DbSet<FileStorage> FileStorages { get; set; }
        public DbSet<Standard> Standards { get; set; }
        public DbSet<StandardSection> StandardSections { get; set; }
        public DbSet<StandardTemplate> StandardTemplates { get; set; }

        // -----------------------------
        // DbSets for join entities
        // -----------------------------
        public DbSet<DocumentRevision> DocumentRevisions { get; set; }
        public DbSet<ProjectDocuments> ProjectDocuments { get; set; }
        public DbSet<ProjectRoles> ProjectRoles { get; set; }
        public DbSet<ProjectAssignments> ProjectAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -----------------------------
            // Apply configurations for main entities
            // -----------------------------
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectStatusConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentStatusConfiguration());
            modelBuilder.ApplyConfiguration(new FileStorageConfiguration());
            modelBuilder.ApplyConfiguration(new StandardConfiguration());
            modelBuilder.ApplyConfiguration(new StandardSectionConfiguration());
            modelBuilder.ApplyConfiguration(new StandardTemplateConfiguration());

            // -----------------------------
            // Apply configurations for join entities
            // -----------------------------
            modelBuilder.ApplyConfiguration(new DocumentRevisionConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectDocumentsConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectRolesConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectAssignmentsConfiguration());
        }
    }
}
