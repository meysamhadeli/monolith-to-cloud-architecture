namespace Unit.Test.Fakes;

using global::Flight.Aircrafts.ValueObjects;
using global::Flight.Airports.ValueObjects;
using global::Flight.Flights.ValueObjects;

public static class FakeFlightCreate
{
    public static global::Flight.Flights.Models.Flight Generate()
    {
        var command = new FakeCreateFlightCommand().Generate();

        return global::Flight.Flights.Models.Flight.Create(FlightId.Of(command.Id), FlightNumber.Of(command.FlightNumber),
            AircraftId.Of(command.AircraftId), AirportId.Of(command.DepartureAirportId), DepartureDate.Of(command.DepartureDate),
            ArriveDate.Of(command.ArriveDate), AirportId.Of(command.ArriveAirportId), DurationMinutes.Of(command.DurationMinutes),
            FlightDate.Of(command.FlightDate), command.Status, Price.Of(command.Price));
    }
}
