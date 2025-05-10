namespace EndToEnd.Test.Fakes;

using AutoBogus;
using global::Flight.Data.Seed;
using global::Flight.Flights.Enums;
using global::Flight.Flights.Features.CreatingFlight.V1;
using MassTransit;

public sealed class FakeCreateFlightMongoCommand : AutoFaker<CreateFlightMongo>
{
    public FakeCreateFlightMongoCommand()
    {
        RuleFor(r => r.Id, _ => NewId.NextGuid());
        RuleFor(r => r.FlightNumber, r => "12FF");
        RuleFor(r => r.DepartureAirportId, _ => InitialData.Airports.First().Id);
        RuleFor(r => r.ArriveAirportId, _ => InitialData.Airports.Last().Id);
        RuleFor(r => r.Status, _ => FlightStatus.Flying);
        RuleFor(r => r.AircraftId, _ => InitialData.Aircrafts.First().Id);
        RuleFor(r => r.IsDeleted, _ => false);
    }
}

