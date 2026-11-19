using RoomHub.Api.Services;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed record GetBookingResponse(Guid Id, Guid RoomId, string BookedBy,
    DateTime StartUtc, DateTime EndUtc, int Attendees, string Status);

public static class GetBookingEndpoint
{
    public static void MapGetBooking(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/bookings/{id:guid}", (Guid id, BookingService service) =>
        {
            var booking = service.Get(id);
            return Results.Ok(new GetBookingResponse(booking.Id, booking.RoomId, booking.BookedBy,
                booking.StartUtc, booking.EndUtc, booking.Attendees, booking.Status.ToString()));
        }).WithTags("Bookings");
    }
}
