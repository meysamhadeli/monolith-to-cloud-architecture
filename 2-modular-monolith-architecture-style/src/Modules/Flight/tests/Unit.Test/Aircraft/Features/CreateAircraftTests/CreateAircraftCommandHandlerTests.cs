using FluentAssertions;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

namespace Unit.Test.Aircraft.Features.CreateAircraftTests;

using global::Flight.Aircrafts.Features.CreatingAircraft.V1;

[Collection(nameof(UnitTestFixture))]
public class CreateAircraftCommandHandlerTests
{
    private readonly UnitTestFixture _fixture;
    private readonly CreateAircraftHandler _handler;

    public Task<CreateAircraftResult> Act(CreateAircraft command, CancellationToken cancellationToken) =>
    _handler.Handle(command, cancellationToken);

    public CreateAircraftCommandHandlerTests(UnitTestFixture fixture)
    {
        _fixture = fixture;
        _handler = new CreateAircraftHandler(_fixture.DbContext);
    }

    [Fact]
    public async Task handler_with_valid_command_should_create_new_aircraft_and_return_currect_aircraft_dto()
    {
        // Arrange
        var command = new FakeCreateAircraftCommand().Generate();

        // Act
        var response = await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.Aircraft.FindAsync(response?.Id);

        entity?.Should().NotBeNull();
        response?.Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_exception()
    {
        // Arrange
        CreateAircraft command = null;

        // Act
        Func<Task> act = async () => { await Act(command, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
