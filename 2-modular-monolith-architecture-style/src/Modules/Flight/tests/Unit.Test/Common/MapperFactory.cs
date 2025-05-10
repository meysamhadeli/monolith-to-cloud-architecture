using Flight;
using Mapster;
using MapsterMapper;

namespace Unit.Test.Common
{
    public static class MapperFactory
    {
        public static IMapper Create()
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.Scan(typeof(FlightRoot).Assembly);
            IMapper instance = new Mapper(typeAdapterConfig);

            return instance;
        }
    }
}
