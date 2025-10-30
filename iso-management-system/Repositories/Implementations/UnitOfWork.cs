using System.Threading.Tasks;
using iso_management_system.Configurations.Db;
using iso_management_system.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace iso_management_system.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(AppDbContext context ,  ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }

    public AppDbContext Context => _context;

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        _transaction ??= await _context.Database.BeginTransactionAsync();
        return _transaction;
    }
    
    public async Task ExecuteInTransactionAsync(Func<Task> work)
    {
        await BeginTransactionAsync();
        var transactionId = _transaction?.TransactionId.ToString() ?? Guid.NewGuid().ToString();

        _logger.LogInformation("Transaction {TransactionId} started at {StartTime}", transactionId, DateTime.UtcNow);

        try
        {
            await work();

            await SaveChangesAsync();
            await CommitAsync();

            _logger.LogInformation("Transaction {TransactionId} committed successfully at {EndTime}", transactionId, DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction {TransactionId} failed. Rolling back at {ErrorTime}", transactionId, DateTime.UtcNow);
            await RollbackAsync();
            throw;
        }
    }

    public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> work)
    {
        await BeginTransactionAsync();
        var transactionId = _transaction?.TransactionId.ToString() ?? Guid.NewGuid().ToString();

        _logger.LogInformation("Transaction {TransactionId} started at {StartTime}", transactionId, DateTime.UtcNow);

        try
        {
            var result = await work();

            await SaveChangesAsync();
            await CommitAsync();

            _logger.LogInformation("Transaction {TransactionId} committed successfully at {EndTime}", transactionId, DateTime.UtcNow);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction {TransactionId} failed. Rolling back at {ErrorTime}", transactionId, DateTime.UtcNow);
            await RollbackAsync();
            throw;
        }
    }


    

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}