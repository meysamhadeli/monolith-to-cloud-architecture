using BuildingBlocks.TestBase;
using Flight;
using Flight.Data;
using FluentAssertions;
using Integration.Test.Fakes;
using Xunit;

namespace Integration.Test.Seat.Features;

using Api;

public class GetAvailableSeatsTests : FlightIntegrationTestBase
{
    public GetAvailableSeatsTests(
        TestFixture<Program, FlightDbContext, FlightReadDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async Task should_return_available_seats_from_grpc_service()
    {
        // Arrange
        var flightCommand = new FakeCreateFlightMongoCommand().Generate();

        await Fixture.SendAsync(flightCommand);

        var seatCommand = new FakeCreateSeatMongoCommand(flightCommand.Id).Generate();

        await Fixture.SendAsync(seatCommand);

        var flightGrpcClient = new FlightGrpcService.FlightGrpcServiceClient(Fixture.Channel);

        // Act
        var response = await flightGrpcClient.GetAvailableSeatsAsync(new GetAvailableSeatsRequest { FlightId = flightCommand.Id.ToString() });

        // Assert
        response?.Should().NotBeNull();
        response?.SeatDtos?.Count.Should().BeGreaterOrEqualTo(1);
    }
}
