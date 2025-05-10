using FluentAssertions;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

namespace Unit.Test.Airport.Features.CreateAirportTests;

using global::Flight.Airports.Features.CreatingAirport.V1;
using global::Flight.Airports.ValueObjects;

[Collection(nameof(UnitTestFixture))]
public class CreateAirportCommandHandlerTests
{
    private readonly UnitTestFixture _fixture;
    private readonly CreateAirportHandler _handler;


    public CreateAirportCommandHandlerTests(UnitTestFixture fixture)
    {
        _fixture = fixture;
        _handler = new CreateAirportHandler(_fixture.DbContext);
    }

    public Task<CreateAirportResult> Act(CreateAirport command, CancellationToken cancellationToken) =>
        _handler.Handle(command, cancellationToken);

    [Fact]
    public async Task handler_with_valid_command_should_create_new_airport_and_return_currect_airport_dto()
    {
        // Arrange
        var command = new FakeCreateAirportCommand().Generate();

        // Act
        var response = await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.Airports.FindAsync(AirportId.Of(response.Id));

        entity?.Should().NotBeNull();
        response?.Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_exception()
    {
        // Arrange
        CreateAirport command = null;

        // Act
        var act = async () => { await Act(command, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
