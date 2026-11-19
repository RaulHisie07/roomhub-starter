using System.Collections.Concurrent;
using RoomHub.Api.Domain.Bookings;
namespace RoomHub.Api.Infrastructure;
public sealed class InMemoryBookingRepository : IBookingRepository
{
    private readonly ConcurrentDictionary<Guid, Booking> _bookings = new();
    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_bookings.GetValueOrDefault(id));
    }
    public Task<IReadOnlyList<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyList<Booking>>(_bookings.Values
            .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active).ToList());
    }
    public Task<int> CountActiveForParticipantOnDayAsync(string bookedBy, DateTime dayUtc, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_bookings.Values.Count(b => b.Status == BookingStatus.Active
            && b.BookedBy == bookedBy && b.StartUtc.Date == dayUtc.Date));
    }
    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _bookings[booking.Id] = booking;
        return Task.CompletedTask;
    }
}
