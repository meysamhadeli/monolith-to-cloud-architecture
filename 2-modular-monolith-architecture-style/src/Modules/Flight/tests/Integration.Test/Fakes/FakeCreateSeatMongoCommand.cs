namespace Integration.Test.Fakes;

using System;
using AutoBogus;
using global::Flight.Seats.Enums;
using global::Flight.Seats.Features.CreatingSeat.V1;
using MassTransit;

public class FakeCreateSeatMongoCommand : AutoFaker<CreateSeatMongo>
{
    public FakeCreateSeatMongoCommand(Guid flightId)
    {
        RuleFor(r => r.Id, _ => NewId.NextGuid());
        RuleFor(r => r.FlightId, _ => flightId);
        RuleFor(r => r.Class, _ => SeatClass.Economy);
        RuleFor(r => r.Type, _ => SeatType.Middle);
        RuleFor(r => r.IsDeleted, _ => false);
    }
}
