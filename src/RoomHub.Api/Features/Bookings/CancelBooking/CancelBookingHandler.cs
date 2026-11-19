using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Common.Cqrs;

namespace RoomHub.Api.Features.Bookings.CancelBooking;
public sealed class CancelBookingHandler(IBookingRepository bookings, TimeProvider timeProvider, IUnitOfWork unitOfWork) : ICommandHandler<CancelBookingCommand, Result<bool>>
{
    public async Task<Result<bool>> HandleAsync(CancelBookingCommand command, CancellationToken cancellationToken = default)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        var booking = await bookings.GetByIdAsync(command.Id, cancellationToken);
        if (booking is null) return Result<bool>.Failure(new("BookingNotFound", "The booking does not exist.", ErrorType.NotFound));
        var cancelled = booking.Cancel(timeProvider.GetUtcNow());
        if (cancelled.IsFailure) return cancelled;
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return cancelled;
    }
}
