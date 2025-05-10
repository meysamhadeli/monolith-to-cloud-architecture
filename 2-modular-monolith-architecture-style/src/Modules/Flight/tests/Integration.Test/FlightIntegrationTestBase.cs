using Api;
using BuildingBlocks.TestBase;
using Flight.Data;
using Xunit;

namespace Integration.Test;

[Collection(IntegrationTestCollection.Name)]
public class FlightIntegrationTestBase : TestBase<Program, FlightDbContext, FlightReadDbContext>
{
    public FlightIntegrationTestBase(TestFixture<Program, FlightDbContext, FlightReadDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }
}

[CollectionDefinition(Name)]
public class IntegrationTestCollection : ICollectionFixture<TestFixture<Program, FlightDbContext, FlightReadDbContext>>
{
    public const string Name = "Flight Integration Test";
}
