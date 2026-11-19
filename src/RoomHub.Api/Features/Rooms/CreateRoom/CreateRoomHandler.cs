using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Common.Cqrs;

namespace RoomHub.Api.Features.Rooms.CreateRoom;
public sealed class CreateRoomHandler(IRoomRepository rooms, IUnitOfWork unitOfWork) : ICommandHandler<CreateRoomCommand, Result<CreateRoomResponse>>
{
    public async Task<Result<CreateRoomResponse>> HandleAsync(CreateRoomCommand command, CancellationToken cancellationToken = default)
    {
        var created = Room.Create(command.Name, command.Capacity);
        if (created.IsFailure) return Result<CreateRoomResponse>.Failure(created.Error);
        await rooms.AddAsync(created.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<CreateRoomResponse>.Success(new(created.Value.Id, created.Value.Name, created.Value.Capacity));
    }
}
