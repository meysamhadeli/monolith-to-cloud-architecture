namespace Unit.Test.Flight.Features.Domains
{
    using System.Linq;
    using FluentAssertions;
    using global::Flight.Flights.Features.CreatingFlight.V1;
    using Unit.Test.Common;
    using Unit.Test.Fakes;
    using Xunit;

    [Collection(nameof(UnitTestFixture))]
    public class CreateFlightTests
    {
        [Fact]
        public void can_create_valid_flight()
        {
            // Arrange + Act
            var fakeFlight = FakeFlightCreate.Generate();

            // Assert
            fakeFlight.Should().NotBeNull();
        }

        [Fact]
        public void queue_domain_event_on_create()
        {
            // Arrange + Act
            var fakeFlight = FakeFlightCreate.Generate();

            // Assert
            fakeFlight.DomainEvents.Count.Should().Be(1);
            fakeFlight.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(FlightCreatedDomainEvent));
        }
    }
}
