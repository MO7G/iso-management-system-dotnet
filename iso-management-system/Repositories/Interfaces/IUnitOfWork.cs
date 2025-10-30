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
// ⭐️ Reusable Transaction Template Method ⭐️
    Task ExecuteInTransactionAsync(Func<Task> work);

// (optional overload if you want to return a result)
    Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> work);
    // 🔹 Save all changes (if not using transaction)
    Task<int> SaveChangesAsync();

    // 🔹 Expose the shared DbContext when really needed (optional but helpful)
    AppDbContext Context { get; }
}