namespace Unit.Test.Flight.Features.Domains;

using System.Linq;
using FluentAssertions;
using global::Flight.Flights.Features.UpdatingFlight.V1;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class UpdateFlightTests
{
    [Fact]
    public void can_update_valid_flight()
    {
        // Arrange
        var fakeFlight = FakeFlightCreate.Generate();

        // Act
        FakeFlightUpdate.Generate(fakeFlight);

        // Assert
        fakeFlight.Price.Value.Should().Be(1000);
    }

    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeFlight = FakeFlightCreate.Generate();
        fakeFlight.ClearDomainEvents();

        // Act
        FakeFlightUpdate.Generate(fakeFlight);

        // Assert
        fakeFlight.DomainEvents.Count.Should().Be(1);
        fakeFlight.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(FlightUpdatedDomainEvent));
    }
}
