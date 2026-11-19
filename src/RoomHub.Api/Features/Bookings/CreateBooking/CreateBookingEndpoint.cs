using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Common.Http;

namespace RoomHub.Api.Features.Bookings.CreateBooking;
public sealed record CreateBookingRequest(string? BookedBy, DateTimeOffset StartUtc, DateTimeOffset EndUtc, int Attendees);

public static class CreateBookingEndpoint
{
    public static void MapCreateBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms/{roomId:guid}/bookings", async (Guid roomId, CreateBookingRequest request,
            ICommandHandler<CreateBookingCommand, Result<CreateBookingResponse>> handler, CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new CreateBookingCommand(roomId, request.BookedBy, request.StartUtc, request.EndUtc, request.Attendees), cancellationToken);
            return result.ToHttpResult(value => Results.Created($"/api/bookings/{value.Id}", value));
        }).WithTags("Bookings");
    }
}
