using AutoBogus;

namespace Unit.Test.Fakes;

using global::Flight.Airports.Features.CreatingAirport.V1;

public class FakeValidateCreateAirportCommand : AutoFaker<CreateAirport>
{
    public FakeValidateCreateAirportCommand()
    {
        RuleFor(r => r.Code, _ => null);
        RuleFor(r => r.Name, _ => null);
        RuleFor(r => r.Address, _ => null);
    }
}
