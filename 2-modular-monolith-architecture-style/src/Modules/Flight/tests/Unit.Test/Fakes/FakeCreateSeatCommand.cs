using AutoBogus;
using Flight.Seats.Enums;

namespace Unit.Test.Fakes;

using System.Linq;
using global::Flight.Data.Seed;
using global::Flight.Seats.Features.CreatingSeat.V1;
using MassTransit;

public class FakeCreateSeatCommand : AutoFaker<CreateSeat>
{
    public FakeCreateSeatCommand()
    {
        RuleFor(r => r.Id, _ => NewId.NextGuid());
        RuleFor(r => r.FlightId, _ => InitialData.Flights.First().Id);
        RuleFor(r => r.SeatNumber, _ => "F99");
        RuleFor(r => r.Type, _ => SeatType.Window);
        RuleFor(r => r.Class, _ => SeatClass.Economy);
    }
}
