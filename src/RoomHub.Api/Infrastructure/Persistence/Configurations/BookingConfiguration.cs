using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;
namespace RoomHub.Api.Infrastructure.Persistence.Configurations;
public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(booking => booking.Id);
        builder.Property(booking => booking.BookedBy).IsRequired();
        builder.Property(booking => booking.Status).HasConversion<string>();
        // SQLite does not retain DateTime.Kind; restore UTC when reading.
        builder.Property(booking => booking.StartUtc).HasConversion(
            value => value, value => DateTime.SpecifyKind(value, DateTimeKind.Utc));
        builder.Property(booking => booking.EndUtc).HasConversion(
            value => value, value => DateTime.SpecifyKind(value, DateTimeKind.Utc));
        builder.HasOne<Room>().WithMany().HasForeignKey(booking => booking.RoomId);
        builder.HasIndex(booking => new { booking.RoomId, booking.Status, booking.StartUtc });
        builder.HasIndex(booking => new { booking.BookedBy, booking.Status, booking.StartUtc });
    }
}
