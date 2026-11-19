using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Infrastructure.Persistence;
namespace RoomHub.Api.Infrastructure.Repositories;
public sealed class EfRoomRepository(RoomHubDbContext db) : IRoomRepository
{
    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        db.Rooms.AsNoTracking().SingleOrDefaultAsync(room => room.Id == id, cancellationToken);
    public Task AddAsync(Room room, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        db.Rooms.Add(room);
        return Task.CompletedTask;
    }
}
