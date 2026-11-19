using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Common.Cqrs;

namespace RoomHub.Api.Features.Bookings.CreateBooking;
public sealed class CreateBookingHandler(IRoomRepository rooms, IBookingRepository bookings, TimeProvider timeProvider, IUnitOfWork unitOfWork) : ICommandHandler<CreateBookingCommand, Result<CreateBookingResponse>>
{
    public async Task<Result<CreateBookingResponse>> HandleAsync(CreateBookingCommand command, CancellationToken cancellationToken = default)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        var room = await rooms.GetByIdAsync(command.RoomId, cancellationToken);
        if (room is null) return Result<CreateBookingResponse>.Failure(new("RoomNotFound", "The room does not exist.", ErrorType.NotFound));
        var created = Booking.Create(room, command.BookedBy, command.StartUtc, command.EndUtc, command.Attendees, timeProvider.GetUtcNow());
        if (created.IsFailure) return Result<CreateBookingResponse>.Failure(created.Error);
        var booking = created.Value;
        var existing = await bookings.GetActiveForRoomAsync(room.Id, cancellationToken);
        if (existing.Any(b => b.Overlaps(booking)))
            return Result<CreateBookingResponse>.Failure(new("RoomOccupied", "The room is occupied in this interval.", ErrorType.Conflict));
        var count = await bookings.CountActiveForParticipantOnDayAsync(booking.BookedBy, booking.StartUtc.Date, cancellationToken);
        var limit = BookingPolicy.CheckDailyLimit(count);
        if (limit.IsFailure) return Result<CreateBookingResponse>.Failure(limit.Error);
        await bookings.AddAsync(booking, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return Result<CreateBookingResponse>.Success(new(booking.Id, booking.RoomId, booking.Status.ToString()));
    }
}
