using AutoBogus;

namespace Unit.Test.Fakes;

using global::Flight.Aircrafts.Features.CreatingAircraft.V1;

public class FakeValidateCreateAircraftCommand : AutoFaker<CreateAircraft>
{
    public FakeValidateCreateAircraftCommand()
    {
        RuleFor(r => r.ManufacturingYear, _ => 0);
        RuleFor(r => r.Name, _ => null);
        RuleFor(r => r.Model, _ => null);
    }
}
