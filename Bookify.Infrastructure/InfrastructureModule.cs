using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Notifications;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Clock;
using Bookify.Infrastructure.Dapper;
using Bookify.Infrastructure.EfCore;
using Bookify.Infrastructure.EfCore.Repositories;
using Bookify.Infrastructure.Notifications;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure;

internal static class InfrastructureModule
{
    internal static IServiceCollection AddInfrastructureModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        services.AddTransient<IEmailService, EmailService>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString("Default"))
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUserRepository, UserEfCoreRepository>();
        services.AddScoped<IBookingRepository, BookingEfCoreRepository>();
        services.AddScoped<IApartmentRepository, ApartmentEfCoreRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(configuration.GetConnectionString("Default")!));

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        return services;
    }
}