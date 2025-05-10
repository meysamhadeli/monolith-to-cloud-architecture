using Booking.Configuration;
using BookingFlight;
using BuildingBlocks.Web;
using Microsoft.Extensions.DependencyInjection;
using Passenger;

namespace Booking.Extensions.Infrastructure;

using BuildingBlocks.Polly;

public static class GrpcClientExtensions
{
    public static IServiceCollection AddGrpcClients(this IServiceCollection services)
    {
        var grpcOptions = services.GetOptions<GrpcOptions>("Grpc");

        services.AddGrpcClient<FlightGrpcService.FlightGrpcServiceClient>(o =>
        {
            o.Address = new Uri(grpcOptions.FlightAddress);
        })
            .AddGrpcRetryPolicyHandler()
            .AddGrpcCircuitBreakerPolicyHandler();

        services.AddGrpcClient<PassengerGrpcService.PassengerGrpcServiceClient>(o =>
        {
            o.Address = new Uri(grpcOptions.PassengerAddress);
        })
            .AddGrpcRetryPolicyHandler()
            .AddGrpcCircuitBreakerPolicyHandler();

        return services;
    }
}
