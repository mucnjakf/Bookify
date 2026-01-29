using Bookify.Application.Abstractions.Behaviors;
using Bookify.Domain.Bookings;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Application;

internal static class ApplicationModule
{
    internal static IServiceCollection AddApplicationModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(configure =>
        {
            configure.LicenseKey = configuration["MediatR:LicenseKey"]!;
            configure.RegisterServicesFromAssembly(typeof(ApplicationModule).Assembly);

            configure.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configure.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationModule).Assembly);

        services.AddTransient<PricingService>();

        return services;
    }
}