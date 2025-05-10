namespace Unit.Test.Fakes;

using global::Flight.Flights.Models;
using global::Flight.Flights.ValueObjects;

public static class FakeFlightUpdate
{
    public static void Generate(Flight flight)
    {
        flight.Update(flight.Id, flight.FlightNumber, flight.AircraftId, flight.DepartureAirportId, flight.DepartureDate,
            flight.ArriveDate, flight.ArriveAirportId, flight.DurationMinutes, flight.FlightDate, flight.Status, Price.Of(1000), flight.IsDeleted);
    }
}
