using Flight.Data;
using Flight.Flights.Enums;
using Flight.Seats.Enums;
using Microsoft.EntityFrameworkCore;

namespace Unit.Test.Common;

using global::Flight.Aircrafts.ValueObjects;
using global::Flight.Airports.ValueObjects;
using global::Flight.Flights.ValueObjects;
using global::Flight.Seats.ValueObjects;
using MassTransit;
using AirportName = global::Flight.Airports.ValueObjects.Name;
using Name = global::Flight.Aircrafts.ValueObjects.Name;

public static class DbContextFactory
{
    private static readonly Guid _airportId1 = NewId.NextGuid();
    private static readonly Guid _airportId2 = NewId.NextGuid();
    private static readonly Guid _aircraft1 = NewId.NextGuid();
    private static readonly Guid _aircraft2 = NewId.NextGuid();
    private static readonly Guid _aircraft3 = NewId.NextGuid();
    private static readonly Guid _flightId1 = NewId.NextGuid();

    public static FlightDbContext Create()
    {
        var options = new DbContextOptionsBuilder<FlightDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        var context = new FlightDbContext(options, currentUserProvider: null, null);

        // Seed our data
        FlightDataSeeder(context);

        return context;
    }

    private static void FlightDataSeeder(FlightDbContext context)
    {
        var airports = new List<global::Flight.Airports.Models.Airport>
        {
            global::Flight.Airports.Models.Airport.Create(AirportId.Of(_airportId1), AirportName.Of("Lisbon International Airport"), Address.Of("LIS"),
                Code.Of("12988")),
            global::Flight.Airports.Models.Airport.Create(AirportId.Of(_airportId2), AirportName.Of("Sao Paulo International Airport"), Address.Of("BRZ"),
                Code.Of("11200"))
        };

        context.Airports.AddRange(airports);

        var aircrafts = new List<global::Flight.Aircrafts.Models.Aircraft>
        {
            global::Flight.Aircrafts.Models.Aircraft.Create(AircraftId.Of(_aircraft1), Name.Of("Boeing 737"), Model.Of("B737"), ManufacturingYear.Of(2005)),
            global::Flight.Aircrafts.Models.Aircraft.Create(AircraftId.Of(_aircraft2), Name.Of("Airbus 300"), Model.Of("A300"), ManufacturingYear.Of(2000)),
            global::Flight.Aircrafts.Models.Aircraft.Create(AircraftId.Of(_aircraft3), Name.Of("Airbus 320"), Model.Of("A320"), ManufacturingYear.Of(2003))
        };

        context.Aircraft.AddRange(aircrafts);

        var flights = new List<global::Flight.Flights.Models.Flight>
        {
            global::Flight.Flights.Models.Flight.Create(FlightId.Of(_flightId1), FlightNumber.Of( "BD467"), AircraftId.Of(_aircraft1), AirportId.Of( _airportId1),
                DepartureDate.Of( new DateTime(2022, 1, 31, 12, 0, 0)),
                ArriveDate.Of( new DateTime(2022, 1, 31, 14, 0, 0)),
                AirportId.Of( _airportId2), DurationMinutes.Of(120m),
                FlightDate.Of( new DateTime(2022, 1, 31)), FlightStatus.Completed,
                Price.Of(8000))
        };
        context.Flights.AddRange(flights);

        var seats = new List<global::Flight.Seats.Models.Seat>
        {
            global::Flight.Seats.Models.Seat.Create(SeatId.Of( NewId.NextGuid()), SeatNumber.Of("12A"), SeatType.Window, SeatClass.Economy,
                FlightId.Of(_flightId1)),
            global::Flight.Seats.Models.Seat.Create(SeatId.Of(NewId.NextGuid()), SeatNumber.Of("12B"), SeatType.Window, SeatClass.Economy,
                FlightId.Of(_flightId1)),
            global::Flight.Seats.Models.Seat.Create(SeatId.Of(NewId.NextGuid()), SeatNumber.Of("12C"), SeatType.Middle, SeatClass.Economy,
                FlightId.Of(_flightId1)),
            global::Flight.Seats.Models.Seat.Create(SeatId.Of(NewId.NextGuid()), SeatNumber.Of("12D"), SeatType.Middle, SeatClass.Economy,
                FlightId.Of(_flightId1)),
            global::Flight.Seats.Models.Seat.Create(SeatId.Of(NewId.NextGuid()), SeatNumber.Of("12E"), SeatType.Aisle, SeatClass.Economy,
                FlightId.Of(_flightId1)),
            global::Flight.Seats.Models.Seat.Create(SeatId.Of(NewId.NextGuid()), SeatNumber.Of("12F"), SeatType.Aisle, SeatClass.Economy,
                FlightId.Of(_flightId1))
        };

        context.Seats.AddRange(seats);

        context.SaveChanges();
    }

    public static void Destroy(FlightDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
