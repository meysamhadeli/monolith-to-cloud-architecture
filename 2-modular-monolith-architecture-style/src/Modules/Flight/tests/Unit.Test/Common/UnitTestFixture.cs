using Flight.Data;
using MapsterMapper;
using Xunit;

namespace Unit.Test.Common
{
    [CollectionDefinition(nameof(UnitTestFixture))]
    public class FixtureCollection : ICollectionFixture<UnitTestFixture> { }

    public class UnitTestFixture : IDisposable
    {
        public UnitTestFixture()
        {
            Mapper = MapperFactory.Create();
            DbContext = DbContextFactory.Create();
        }

        public IMapper Mapper { get; }
        public FlightDbContext DbContext { get; }

        public void Dispose()
        {
            DbContextFactory.Destroy(DbContext);
        }
    }
}
