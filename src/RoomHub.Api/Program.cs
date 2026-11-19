using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoom;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBooking;
using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Infrastructure.Persistence;
using RoomHub.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(TimeProvider.System);
var connectionString = builder.Configuration.GetConnectionString("RoomHub")
    ?? new SqliteConnectionStringBuilder
    {
        DataSource = Path.Combine(builder.Environment.ContentRootPath, "roomhub.db"),
        DefaultTimeout = 30
    }.ToString();
builder.Services.AddDbContext<RoomHubDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<IRoomRepository, EfRoomRepository>();
builder.Services.AddScoped<IBookingRepository, EfBookingRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<ICommandHandler<CreateRoomCommand, Result<CreateRoomResponse>>, CreateRoomHandler>();
builder.Services.AddScoped<IQueryHandler<GetRoomQuery, Result<GetRoomResponse>>, GetRoomHandler>();
builder.Services.AddScoped<ICommandHandler<CreateBookingCommand, Result<CreateBookingResponse>>, CreateBookingHandler>();
builder.Services.AddScoped<IQueryHandler<GetBookingQuery, Result<GetBookingResponse>>, GetBookingHandler>();
builder.Services.AddScoped<ICommandHandler<CancelBookingCommand, Result<bool>>, CancelBookingHandler>();

var app = builder.Build();
await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RoomHubDbContext>();
    await db.Database.EnsureCreatedAsync();
}
app.MapGet("/", () => "RoomHub API is running.");
app.MapCreateRoom();
app.MapGetRoom();
app.MapCreateBooking();
app.MapGetBooking();
app.MapCancelBooking();
app.Run();
