using iso_management_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iso_management_system.Configurations.Db
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            // Primary Key
            builder.HasKey(c => c.CustomerID);

            // Properties
            builder.Property(c => c.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(200);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(c => c.ModifiedAt)
                .HasDefaultValueSql("GETDATE()");

            // -----------------------------
            // 🔹 Customer → Projects (One-to-Many)
            // -----------------------------
            builder.HasMany(c => c.Projects)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // -----------------------------
            // 🔹 Customer → FileStorage (One-to-Many)
            // -----------------------------
            builder.HasMany(c => c.UploadedFiles)
                .WithOne(f => f.UploadedByCustomer)
                .HasForeignKey(f => f.UploadedByCustomerID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}