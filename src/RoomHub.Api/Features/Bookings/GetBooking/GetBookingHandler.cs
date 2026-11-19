using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Common.Cqrs;

namespace RoomHub.Api.Features.Bookings.GetBooking;
public sealed class GetBookingHandler(IBookingRepository bookings) : IQueryHandler<GetBookingQuery, Result<GetBookingResponse>>
{
    public async Task<Result<GetBookingResponse>> HandleAsync(GetBookingQuery command, CancellationToken cancellationToken = default)
    {
        var booking = await bookings.GetByIdAsync(command.Id, cancellationToken);
        return booking is null ? Result<GetBookingResponse>.Failure(new("BookingNotFound", "The booking does not exist.", ErrorType.NotFound))
            : Result<GetBookingResponse>.Success(new(booking.Id, booking.RoomId, booking.BookedBy, booking.StartUtc, booking.EndUtc, booking.Attendees, booking.Status.ToString()));
    }
}
