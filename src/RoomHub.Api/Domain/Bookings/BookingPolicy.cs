using RoomHub.Api.Domain.Common;
namespace RoomHub.Api.Domain.Bookings;
public static class BookingPolicy
{
    public static Result<bool> CheckDailyLimit(int activeCount) => activeCount >= 2
        ? Result<bool>.Failure(new("DailyBookingLimit", "A participant can have at most two active bookings per UTC day.", ErrorType.Conflict))
        : Result<bool>.Success(true);
}
