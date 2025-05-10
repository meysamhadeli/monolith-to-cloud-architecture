namespace Unit.Test.Flight.Features.Handlers.CreateFlight;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using global::Flight.Flights.Features.CreatingFlight.V1;
using global::Flight.Flights.ValueObjects;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class CreateFlightCommandHandlerTests
{
    private readonly UnitTestFixture _fixture;
    private readonly CreateFlightHandler _handler;

    public Task<CreateFlightResult> Act(CreateFlight command, CancellationToken cancellationToken) =>
        _handler.Handle(command, cancellationToken);

    public CreateFlightCommandHandlerTests(UnitTestFixture fixture)
    {
        _fixture = fixture;
        _handler = new CreateFlightHandler(fixture.DbContext);
    }

    [Fact]
    public async Task handler_with_valid_command_should_create_new_flight_and_return_currect_flight_dto()
    {
        // Arrange
        var command = new FakeCreateFlightCommand().Generate();

        // Act
        var response = await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.Flights.FindAsync(FlightId.Of(response.Id));

        entity?.Should().NotBeNull();
        response?.Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_exception()
    {
        // Arrange
        CreateFlight command = null;

        // Act
        Func<Task> act = async () => { await Act(command, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
