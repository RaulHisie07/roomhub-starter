using RoomHub.Api.Domain.Common;
namespace RoomHub.Api.Domain.Rooms;
public sealed class Room
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Capacity { get; private set; }
    private Room() { }
    public static Result<Room> Create(string? name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Room>.Failure(new("InvalidName", "Room name is required.", ErrorType.Validation));
        if (capacity <= 0)
            return Result<Room>.Failure(new("InvalidCapacity", "Capacity must be positive.", ErrorType.Validation));
        return Result<Room>.Success(new Room { Id = Guid.NewGuid(), Name = name.Trim(), Capacity = capacity });
    }
    public TimeSpan MaximumBookingDuration => TimeSpan.FromHours(Capacity > 10 ? 2 : 4);
}
