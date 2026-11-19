using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
namespace RoomHub.Api.Domain.Bookings;
public enum BookingStatus { Active, Cancelled }
public sealed class Booking
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }
    private Booking() { }
    public static Result<Booking> Create(Room room, string? bookedBy, DateTimeOffset start,
        DateTimeOffset end, int attendees, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(bookedBy)) return Invalid("InvalidBookedBy", "BookedBy is required.");
        if (end <= start) return Invalid("InvalidRange", "The end must be after the start.");
        if (attendees <= 0 || attendees > room.Capacity)
            return Invalid("InvalidAttendees", "Attendees must be positive and at most the room capacity.");
        if (start < now) return Invalid("StartInPast", "A booking cannot start in the past.");
        if (end - start > room.MaximumBookingDuration)
            return Invalid("BookingTooLong", $"Maximum duration for this room is {room.MaximumBookingDuration.TotalHours} hours.");
        return Result<Booking>.Success(new Booking
        {
            Id = Guid.NewGuid(), RoomId = room.Id, BookedBy = bookedBy.Trim(),
            StartUtc = start.UtcDateTime, EndUtc = end.UtcDateTime, Attendees = attendees, Status = BookingStatus.Active
        });
    }
    public bool Overlaps(Booking other) => Status == BookingStatus.Active && other.Status == BookingStatus.Active
        && RoomId == other.RoomId && StartUtc < other.EndUtc && other.StartUtc < EndUtc;
    public Result<bool> Cancel(DateTimeOffset now)
    {
        if (Status == BookingStatus.Cancelled)
            return Result<bool>.Failure(new("AlreadyCancelled", "The booking is already cancelled.", ErrorType.Conflict));
        if (StartUtc - now.UtcDateTime < TimeSpan.FromMinutes(60))
            return Result<bool>.Failure(new("CancellationTooLate", "Cancellation requires at least 60 minutes before the start.", ErrorType.Conflict));
        Status = BookingStatus.Cancelled;
        return Result<bool>.Success(true);
    }
    private static Result<Booking> Invalid(string code, string message) =>
        Result<Booking>.Failure(new(code, message, ErrorType.Validation));
}
