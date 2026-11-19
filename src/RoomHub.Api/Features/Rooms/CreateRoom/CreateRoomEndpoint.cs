using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Common.Http;

namespace RoomHub.Api.Features.Rooms.CreateRoom;
public sealed record CreateRoomRequest(string? Name, int Capacity);

public static class CreateRoomEndpoint
{
    public static void MapCreateRoom(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms", async (CreateRoomRequest request,
            ICommandHandler<CreateRoomCommand, Result<CreateRoomResponse>> handler, CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new CreateRoomCommand(request.Name, request.Capacity), cancellationToken);
            return result.ToHttpResult(value => Results.Created($"/api/rooms/{value.Id}", value));
        }).WithTags("Rooms");
    }
}
