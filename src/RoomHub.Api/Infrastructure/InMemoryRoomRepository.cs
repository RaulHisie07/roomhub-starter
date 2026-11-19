using System.Collections.Concurrent;
using RoomHub.Api.Domain.Rooms;
namespace RoomHub.Api.Infrastructure;
public sealed class InMemoryRoomRepository : IRoomRepository
{
    private readonly ConcurrentDictionary<Guid, Room> _rooms = new();
    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_rooms.GetValueOrDefault(id));
    }
    public Task AddAsync(Room room, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _rooms[room.Id] = room;
        return Task.CompletedTask;
    }
}
