using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Contracts.EventBus.Messages;
using BuildingBlocks.TestBase;
using Api;
using Flight.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Integration.Test.Flight.Features;

using global::Flight.Data.Seed;
using global::Flight.Flights.Features.DeletingFlight.V1;
using global::Flight.Flights.Models;
using global::Flight.Flights.ValueObjects;

public class DeleteFlightTests : FlightIntegrationTestBase
{
    public DeleteFlightTests(
        TestFixture<Program, FlightDbContext, FlightReadDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async Task should_delete_flight_from_db()
    {
        // Arrange
        var flightEntity = await Fixture.FindAsync<Flight, FlightId>(InitialData.Flights.First().Id);
        var command = new DeleteFlight(flightEntity.Id.Value);

        // Act
        await Fixture.SendAsync(command);
        var deletedFlight = (await Fixture.ExecuteDbContextAsync(db => db.Flights
                .Where(x => x.Id == FlightId.Of(command.Id))
                .IgnoreQueryFilters()
                .ToListAsync())
            ).FirstOrDefault();

        // Assert
        deletedFlight?.IsDeleted.Should().BeTrue();

        (await Fixture.WaitForPublishing<FlightDeleted>()).Should().Be(true);
    }
}
