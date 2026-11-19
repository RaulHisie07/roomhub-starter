using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Common.Cqrs;

namespace RoomHub.Api.Features.Bookings.CancelBooking;
public sealed class CancelBookingHandler(IBookingRepository bookings, TimeProvider timeProvider) : ICommandHandler<CancelBookingCommand, Result<bool>>
{
    public async Task<Result<bool>> HandleAsync(CancelBookingCommand command, CancellationToken cancellationToken = default)
    {
        var booking = await bookings.GetByIdAsync(command.Id, cancellationToken);
        if (booking is null) return Result<bool>.Failure(new("BookingNotFound", "The booking does not exist.", ErrorType.NotFound));
        return booking.Cancel(timeProvider.GetUtcNow());
    }
}
