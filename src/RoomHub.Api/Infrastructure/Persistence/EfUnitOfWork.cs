using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RoomHub.Api.Domain.Common;
namespace RoomHub.Api.Infrastructure.Persistence;
public sealed class EfUnitOfWork(RoomHubDbContext db) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => db.SaveChangesAsync(cancellationToken);
    public async Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await db.Database.OpenConnectionAsync(cancellationToken);
        var connection = (SqliteConnection)db.Database.GetDbConnection();
        // Reserve the SQLite writer before reading availability or daily counts.
        // Other writers wait, then read the committed state, even in another API process.
        var transaction = connection.BeginTransaction(deferred: false);
        try
        {
            var enlisted = await db.Database.UseTransactionAsync(transaction, cancellationToken);
            return new UnitOfWorkTransaction(enlisted!, transaction);
        }
        catch
        {
            await transaction.DisposeAsync();
            throw;
        }
    }
    private sealed class UnitOfWorkTransaction(IDbContextTransaction enlisted, SqliteTransaction transaction) : IUnitOfWorkTransaction
    {
        public Task CommitAsync(CancellationToken cancellationToken = default) => enlisted.CommitAsync(cancellationToken);
        public async ValueTask DisposeAsync()
        {
            // Disposing an uncommitted transaction rolls back any changes.
            await enlisted.DisposeAsync();
            await transaction.DisposeAsync();
        }
    }
}
