using AutoBogus;

namespace Integration.Test.Fakes;

using global::Flight.Airports.Features.CreatingAirport.V1;
using MassTransit;

public class FakeCreateAirportCommand : AutoFaker<CreateAirport>
{
    public FakeCreateAirportCommand()
    {
        RuleFor(r => r.Id, _ => NewId.NextGuid());
    }
}
