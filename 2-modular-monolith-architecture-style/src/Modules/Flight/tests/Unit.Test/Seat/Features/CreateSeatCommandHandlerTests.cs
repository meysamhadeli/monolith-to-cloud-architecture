using FluentAssertions;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

namespace Unit.Test.Seat.Features;

using global::Flight.Seats.Features.CreatingSeat.V1;
using global::Flight.Seats.ValueObjects;

[Collection(nameof(UnitTestFixture))]
public class CreateSeatCommandHandlerTests
{
    private readonly UnitTestFixture _fixture;
    private readonly CreateSeatCommandHandler _handler;


    public CreateSeatCommandHandlerTests(UnitTestFixture fixture)
    {
        _fixture = fixture;
        _handler = new CreateSeatCommandHandler(_fixture.DbContext);
    }

    public Task<CreateSeatResult> Act(CreateSeat command, CancellationToken cancellationToken)
    {
        return _handler.Handle(command, cancellationToken);
    }

    [Fact]
    public async Task handler_with_valid_command_should_create_new_seat_and_return_currect_seat_dto()
    {
        // Arrange
        var command = new FakeCreateSeatCommand().Generate();

        // Act
        var response = await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.Seats.FindAsync(SeatId.Of(response.Id));

        entity?.Should().NotBeNull();
        response?.Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_exception()
    {
        // Arrange
        CreateSeat command = null;

        // Act
        var act = async () => { await Act(command, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
