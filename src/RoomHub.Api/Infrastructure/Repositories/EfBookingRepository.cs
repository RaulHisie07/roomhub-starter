using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Infrastructure.Persistence;
namespace RoomHub.Api.Infrastructure.Repositories;
public sealed class EfBookingRepository(RoomHubDbContext db) : IBookingRepository
{
    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        db.Bookings.SingleOrDefaultAsync(booking => booking.Id == id, cancellationToken);
    public async Task<IReadOnlyList<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken = default) =>
        await db.Bookings.AsNoTracking().Where(booking => booking.RoomId == roomId && booking.Status == BookingStatus.Active)
            .ToListAsync(cancellationToken);
    public Task<int> CountActiveForParticipantOnDayAsync(string bookedBy, DateTime dayUtc, CancellationToken cancellationToken = default)
    {
        var start = dayUtc.Date;
        var end = start.AddDays(1);
        return db.Bookings.CountAsync(booking => booking.BookedBy == bookedBy
            && booking.Status == BookingStatus.Active && booking.StartUtc >= start && booking.StartUtc < end, cancellationToken);
    }
    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        db.Bookings.Add(booking);
        return Task.CompletedTask;
    }
}
