namespace Unit.Test.Flight.Features.Handlers.CreateFlight;

using FluentAssertions;
using FluentValidation.TestHelper;
using global::Flight.Flights.Features.CreatingFlight.V1;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class CreateFlightCommandValidatorTests
{
    [Fact]
    public void is_valid_should_be_false_when_have_invalid_parameter()
    {
        // Arrange
        var command = new FakeValidateCreateFlightCommand().Generate();
        var validator = new CreateFlightValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Price);
        result.ShouldHaveValidationErrorFor(x => x.Status);
        result.ShouldHaveValidationErrorFor(x => x.AircraftId);
        result.ShouldHaveValidationErrorFor(x => x.DepartureAirportId);
        result.ShouldHaveValidationErrorFor(x => x.ArriveAirportId);
        result.ShouldHaveValidationErrorFor(x => x.DurationMinutes);
        result.ShouldHaveValidationErrorFor(x => x.FlightDate);
    }
}
