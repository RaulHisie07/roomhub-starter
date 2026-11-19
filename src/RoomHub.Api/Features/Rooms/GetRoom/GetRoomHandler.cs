using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Common.Cqrs;

namespace RoomHub.Api.Features.Rooms.GetRoom;
public sealed class GetRoomHandler(IRoomRepository rooms) : IQueryHandler<GetRoomQuery, Result<GetRoomResponse>>
{
    public async Task<Result<GetRoomResponse>> HandleAsync(GetRoomQuery command, CancellationToken cancellationToken = default)
    {
        var room = await rooms.GetByIdAsync(command.Id, cancellationToken);
        return room is null ? Result<GetRoomResponse>.Failure(new("RoomNotFound", "The room does not exist.", ErrorType.NotFound))
            : Result<GetRoomResponse>.Success(new(room.Id, room.Name, room.Capacity));
    }
}
