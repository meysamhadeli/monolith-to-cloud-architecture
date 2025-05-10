using AutoBogus;

namespace Integration.Test.Fakes;

using global::Flight.Flights.Features.UpdatingFlight.V1;

public class FakeUpdateFlightCommand : AutoFaker<UpdateFlight>
{
    public FakeUpdateFlightCommand(global::Flight.Flights.Models.Flight flight)
    {
        RuleFor(r => r.Id, _ => flight.Id);
        RuleFor(r => r.DepartureAirportId, _ => flight.DepartureAirportId);
        RuleFor(r => r.ArriveAirportId, _ => flight.ArriveAirportId);
        RuleFor(r => r.AircraftId, _ => flight.AircraftId);
        RuleFor(r => r.FlightNumber, r => r.Random.Number(1000, 2000).ToString());
        RuleFor(r => r.Price, _ => 800);
        RuleFor(r => r.Status, _ => flight.Status);
        RuleFor(r => r.ArriveDate, _ => flight.ArriveDate);
    }
}
