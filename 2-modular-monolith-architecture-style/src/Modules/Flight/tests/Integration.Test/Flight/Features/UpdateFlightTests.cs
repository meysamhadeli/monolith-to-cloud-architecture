using System.Threading.Tasks;
using BuildingBlocks.Contracts.EventBus.Messages;
using BuildingBlocks.TestBase;
using Api;
using Flight.Data;
using FluentAssertions;
using Integration.Test.Fakes;
using Xunit;

namespace Integration.Test.Flight.Features;

using System.Linq;
using global::Flight.Data.Seed;
using global::Flight.Flights.Models;
using global::Flight.Flights.ValueObjects;

public class UpdateFlightTests : FlightIntegrationTestBase
{
    public UpdateFlightTests(
        TestFixture<Program, FlightDbContext, FlightReadDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async Task should_update_flight_to_db_and_publish_message_to_broker()
    {
        // Arrange
        var flightEntity = await Fixture.FindAsync<Flight, FlightId>(InitialData.Flights.First().Id);
        var command = new FakeUpdateFlightCommand(flightEntity).Generate();

        // Act
        var response = await Fixture.SendAsync(command);

        // Assert
        response.Should().NotBeNull();
        response?.Id.Should().Be(flightEntity.Id);

        (await Fixture.WaitForPublishing<FlightUpdated>()).Should().Be(true);
    }
}
