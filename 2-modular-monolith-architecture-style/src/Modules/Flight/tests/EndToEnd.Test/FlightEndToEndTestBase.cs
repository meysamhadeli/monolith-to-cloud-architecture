using Api;
using BuildingBlocks.TestBase;
using Flight.Data;
using Xunit;

namespace EndToEnd.Test;

[Collection(EndToEndTestCollection.Name)]
public class FlightEndToEndTestBase : TestBase<Program, FlightDbContext, FlightReadDbContext>
{
    public FlightEndToEndTestBase(TestFixture<Program, FlightDbContext, FlightReadDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }
}

[CollectionDefinition(Name)]
public class EndToEndTestCollection : ICollectionFixture<TestFixture<Program, FlightDbContext, FlightReadDbContext>>
{
    public const string Name = "Flight EndToEnd Test";
}
