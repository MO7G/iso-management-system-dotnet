using System;
using System.Threading.Tasks;
using iso_management_system.Configurations.Db;
using Microsoft.EntityFrameworkCore.Storage;

namespace iso_management_system.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // 🔹 Transaction control
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();

    // 🔹 Save all changes (if not using transaction)
    Task<int> SaveChangesAsync();

    // 🔹 Expose the shared DbContext when really needed (optional but helpful)
    AppDbContext Context { get; }
}