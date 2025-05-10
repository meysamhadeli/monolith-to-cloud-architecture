namespace Booking.Booking.Features.CreatingBook.V1;

using Ardalis.GuardClauses;
using BookingFlight;
using BuildingBlocks.Core;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Core.Model;
using BuildingBlocks.EventStoreDB.Repository;
using BuildingBlocks.Web;
using Duende.IdentityServer.EntityFramework.Entities;
using Exceptions;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Passenger;
using ValueObjects;

public record CreateBooking(Guid PassengerId, Guid FlightId, string Description) : ICommand<CreateBookingResult>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record CreateBookingResult(ulong Id);

public record BookingCreatedDomainEvent(Guid Id, PassengerInfo PassengerInfo, Trip Trip) : Entity<Guid>, IDomainEvent;

public record CreateBookingRequestDto(Guid PassengerId, Guid FlightId, string Description);

public record CreateBookingResponseDto(ulong Id);

public class CreateBookingEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/booking", async (CreateBookingRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                var command = mapper.Map<CreateBooking>(request);

                var result = await mediator.Send(command, cancellationToken);

                var response = result.Adapt<CreateBookingResponseDto>();

                return Results.Ok(response);
            })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("CreateBooking")
            .WithApiVersionSet(builder.NewApiVersionSet("Booking").Build())
            .Produces<CreateBookingResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Booking")
            .WithDescription("Create Booking")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class CreateBookingValidator : AbstractValidator<CreateBooking>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.FlightId).NotNull().WithMessage("FlightId is required!");
        RuleFor(x => x.PassengerId).NotNull().WithMessage("PassengerId is required!");
    }
}

internal class CreateBookingCommandHandler : ICommandHandler<CreateBooking, CreateBookingResult>
{
    private readonly IEventStoreDBRepository<Models.Booking> _eventStoreDbRepository;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly FlightGrpcService.FlightGrpcServiceClient _flightGrpcServiceClient;
    private readonly PassengerGrpcService.PassengerGrpcServiceClient _passengerGrpcServiceClient;

    public CreateBookingCommandHandler(IEventStoreDBRepository<Models.Booking> eventStoreDbRepository,
        ICurrentUserProvider currentUserProvider,
        IEventDispatcher eventDispatcher,
        FlightGrpcService.FlightGrpcServiceClient flightGrpcServiceClient,
        PassengerGrpcService.PassengerGrpcServiceClient passengerGrpcServiceClient)
    {
        _eventStoreDbRepository = eventStoreDbRepository;
        _currentUserProvider = currentUserProvider;
        _eventDispatcher = eventDispatcher;
        _flightGrpcServiceClient = flightGrpcServiceClient;
        _passengerGrpcServiceClient = passengerGrpcServiceClient;
    }

    public async Task<CreateBookingResult> Handle(CreateBooking command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var flight =
            await _flightGrpcServiceClient.GetByIdAsync(new BookingFlight.GetByIdRequest { Id = command.FlightId.ToString() }, cancellationToken: cancellationToken);

        if (flight is null)
        {
            throw new FlightNotFoundException();
        }

        var passenger =
            await _passengerGrpcServiceClient.GetByIdAsync(new Passenger.GetByIdRequest { Id = command.PassengerId.ToString() }, cancellationToken: cancellationToken);

        var emptySeat = (await _flightGrpcServiceClient
                .GetAvailableSeatsAsync(new GetAvailableSeatsRequest { FlightId = command.FlightId.ToString() }, cancellationToken: cancellationToken)
                .ResponseAsync)
            ?.SeatDtos?.FirstOrDefault();

        var reservation = await _eventStoreDbRepository.Find(command.Id, cancellationToken);

        if (reservation is not null && !reservation.IsDeleted)
        {
            throw new BookingAlreadyExistException();
        }

        var aggrigate = Models.Booking.Create(command.Id, PassengerInfo.Of(passenger.PassengerDto?.Name), Trip.Of(
                flight.FlightDto.FlightNumber, new Guid(flight.FlightDto.AircraftId),
                new Guid(flight.FlightDto.DepartureAirportId),
                new Guid(flight.FlightDto.ArriveAirportId), flight.FlightDto.FlightDate.ToDateTime(),
                (decimal)flight.FlightDto.Price, command.Description,
                emptySeat?.SeatNumber),
            false, _currentUserProvider.GetCurrentUserId());

        await _eventDispatcher.SendAsync(aggrigate.DomainEvents, cancellationToken: cancellationToken);

        await _flightGrpcServiceClient.ReserveSeatAsync(new ReserveSeatRequest
        {
            FlightId = flight.FlightDto.Id,
            SeatNumber = emptySeat?.SeatNumber
        }, cancellationToken: cancellationToken);

        var result = await _eventStoreDbRepository.Add(
            aggrigate,
            cancellationToken);

        return new CreateBookingResult(result);
    }
}
