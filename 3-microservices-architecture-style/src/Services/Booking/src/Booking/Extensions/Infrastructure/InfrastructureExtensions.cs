using System.Threading.RateLimiting;
using Booking.Data;
using BuildingBlocks.Core;
using BuildingBlocks.EventStoreDB;
using BuildingBlocks.HealthCheck;
using BuildingBlocks.Jwt;
using BuildingBlocks.Logging;
using BuildingBlocks.Mapster;
using BuildingBlocks.MassTransit;
using BuildingBlocks.Mongo;
using BuildingBlocks.OpenApi;
using BuildingBlocks.OpenTelemetryCollector;
using BuildingBlocks.PersistMessageProcessor;
using BuildingBlocks.ProblemDetails;
using BuildingBlocks.Web;
using Figgle;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Booking.Extensions.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var env = builder.Environment;

        builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        builder.Services.AddScoped<IEventMapper, BookingEventMapper>();
        builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        var appOptions = builder.Services.GetOptions<AppOptions>(nameof(AppOptions));

        Console.WriteLine(FiggleFonts.Standard.Render(appOptions.Name));

        builder.Services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 10,
                        QueueLimit = 0,
                        Window = TimeSpan.FromMinutes(1)
                    }));
        });

        builder.Services.AddPersistMessageProcessor();
        builder.AddMongoDbContext<BookingReadDbContext>();

        builder.Services.AddEndpointsApiExplorer();
        builder.AddCustomSerilog(env);
        builder.Services.AddJwt();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAspnetOpenApi();
        builder.Services.AddCustomVersioning();
        builder.Services.AddCustomMediatR();
        builder.Services.AddValidatorsFromAssembly(typeof(BookingRoot).Assembly);
        builder.Services.AddProblemDetails();
        builder.Services.AddCustomMapster(typeof(BookingRoot).Assembly);
        builder.Services.AddCustomHealthCheck();
        builder.Services.AddCustomMassTransit(env, TransportType.RabbitMq, typeof(BookingRoot).Assembly);
        builder.AddCustomObservability();
        builder.Services.AddTransient<AuthHeaderHandler>();

        // ref: https://github.com/oskardudycz/EventSourcing.NetCore/tree/main/Sample/EventStoreDB/ECommerce
        builder.Services.AddEventStore(configuration, typeof(BookingRoot).Assembly)
            .AddEventStoreDBSubscriptionToAll();

        builder.Services.AddGrpcClients();

        return builder;
    }


    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        var env = app.Environment;
        var appOptions = app.GetOptions<AppOptions>(nameof(AppOptions));

        app.UseCustomObservability();

        app.UseCustomProblemDetails();
        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = LogEnrichHelper.EnrichFromRequest;
        });
        app.UseCorrelationId();
        app.UseCustomHealthCheck();
        app.MapGet("/", x => x.Response.WriteAsync(appOptions.Name));

        if (env.IsDevelopment())
        {
            app.UseAspnetOpenApi();
        }

        return app;
    }
}
