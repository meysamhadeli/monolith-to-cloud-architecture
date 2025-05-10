using BuildingBlocks.EFCore;
using Flight.Aircrafts.Models;
using Flight.Airports.Models;
using Flight.Data;
using Flight.Data.Seed;
using Flight.Flights.Models;
using Flight.Seats.Models;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Integration.Test;

public class FlightTestDataSeeder(
    FlightDbContext flightDbContext,
    FlightReadDbContext flightReadDbContext,
    IMapper mapper
) : ITestDataSeeder
{
    public async Task SeedAllAsync()
    {
        await SeedAirportAsync();
        await SeedAircraftAsync();
        await SeedFlightAsync();
        await SeedSeatAsync();
    }

    private async Task SeedAirportAsync()
    {
        if (!await EntityFrameworkQueryableExtensions.AnyAsync(flightDbContext.Airports))
        {
            await flightDbContext.Airports.AddRangeAsync(InitialData.Airports);
            await flightDbContext.SaveChangesAsync();

            if (!await MongoQueryable.AnyAsync(flightReadDbContext.Airport.AsQueryable()))
            {
                await flightReadDbContext.Airport.InsertManyAsync(mapper.Map<List<AirportReadModel>>(InitialData.Airports));
            }
        }
    }

    private async Task SeedAircraftAsync()
    {
        if (!await EntityFrameworkQueryableExtensions.AnyAsync(flightDbContext.Aircraft))
        {
            await flightDbContext.Aircraft.AddRangeAsync(InitialData.Aircrafts);
            await flightDbContext.SaveChangesAsync();

            if (!await MongoQueryable.AnyAsync(flightReadDbContext.Aircraft.AsQueryable()))
            {
                await flightReadDbContext.Aircraft.InsertManyAsync(mapper.Map<List<AircraftReadModel>>(InitialData.Aircrafts));
            }
        }
    }


    private async Task SeedSeatAsync()
    {
        if (!await EntityFrameworkQueryableExtensions.AnyAsync(flightDbContext.Seats))
        {
            await flightDbContext.Seats.AddRangeAsync(InitialData.Seats);
            await flightDbContext.SaveChangesAsync();

            if (!await MongoQueryable.AnyAsync(flightReadDbContext.Seat.AsQueryable()))
            {
                await flightReadDbContext.Seat.InsertManyAsync(mapper.Map<List<SeatReadModel>>(InitialData.Seats));
            }
        }
    }

    private async Task SeedFlightAsync()
    {
        if (!await EntityFrameworkQueryableExtensions.AnyAsync(flightDbContext.Flights))
        {
            await flightDbContext.Flights.AddRangeAsync(InitialData.Flights);
            await flightDbContext.SaveChangesAsync();

            if (!await MongoQueryable.AnyAsync(flightReadDbContext.Flight.AsQueryable()))
            {
                await flightReadDbContext.Flight.InsertManyAsync(mapper.Map<List<FlightReadModel>>(InitialData.Flights));
            }
        }
    }
}
