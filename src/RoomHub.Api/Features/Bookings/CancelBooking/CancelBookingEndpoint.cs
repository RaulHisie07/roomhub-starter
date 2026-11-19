using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Common.Http;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void MapCancelBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings/{id:guid}/cancel", async (Guid id,
            ICommandHandler<CancelBookingCommand, Result<bool>> handler, CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new CancelBookingCommand(id), cancellationToken);
            return result.ToHttpResult(value => Results.NoContent());
        }).WithTags("Bookings");
    }
}
