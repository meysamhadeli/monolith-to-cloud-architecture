using AutoBogus;
using Flight.Seats.Enums;

namespace Unit.Test.Fakes;

using System;
using global::Flight.Seats.Features.CreatingSeat.V1;

public class FakeValidateCreateSeatCommand : AutoFaker<CreateSeat>
{
    public FakeValidateCreateSeatCommand()
    {
        RuleFor(r => r.SeatNumber, _ => null);
        RuleFor(r => r.FlightId, _ => Guid.Empty);
        RuleFor(r => r.Class, _ => (SeatClass)10);
    }
}
