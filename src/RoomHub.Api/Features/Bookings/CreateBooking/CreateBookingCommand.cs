namespace RoomHub.Api.Features.Bookings.CreateBooking;
public sealed record CreateBookingCommand(Guid RoomId, string? BookedBy, DateTimeOffset StartUtc, DateTimeOffset EndUtc, int Attendees);
