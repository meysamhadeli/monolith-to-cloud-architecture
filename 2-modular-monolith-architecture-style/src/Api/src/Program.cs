using Api.Extensions;
using Booking.Extensions.Infrastructure;
using BuildingBlocks.Web;
using Flight.Extensions.Infrastructure;
using Identity.Extensions.Infrastructure;
using Passenger.Extensions.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedInfrastructure();

builder.AddIdentityModules();
builder.AddPassengerModules();
builder.AddBookingModules();
builder.AddFlightModules();

var app = builder.Build();

// ref: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-7.0#routing-basics
app.UseAuthentication();
app.UseAuthorization();

app.UseFlightModules();
app.UseIdentityModules();
app.UsePassengerModules();
app.UseBookingModules();

app.UserSharedInfrastructure();
app.MapMinimalEndpoints();

app.Run();

namespace Api
{
    public partial class Program
    {
    }
}
