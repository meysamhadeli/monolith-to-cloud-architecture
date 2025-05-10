using FluentAssertions;
using FluentValidation.TestHelper;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

namespace Unit.Test.Aircraft.Features.CreateAircraftTests;

using global::Flight.Aircrafts.Features.CreatingAircraft.V1;

[Collection(nameof(UnitTestFixture))]
public class CreateAircraftCommandValidatorTests
{
    [Fact]
    public void is_valid_should_be_false_when_have_invalid_parameter()
    {
        // Arrange
        var command = new FakeValidateCreateAircraftCommand().Generate();
        var validator = new CreateAircraftValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Model);
        result.ShouldHaveValidationErrorFor(x => x.ManufacturingYear);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}
