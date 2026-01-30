using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using FluentValidation;

namespace Bookify.Application.Bookings.ReserveBooking;

public sealed record ReserveBookingCommand(
    Guid ApartmentId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate)
    : ICommand<Guid>;

internal sealed class ReserveBookingCommandHandler(
    IUserRepository userRepository,
    IApartmentRepository apartmentRepository,
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork,
    PricingService pricingService,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<ReserveBookingCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ReserveBookingCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        Apartment? apartment = await apartmentRepository.GetByIdAsync(command.ApartmentId, cancellationToken);

        if (apartment is null)
        {
            return Result.Failure<Guid>(ApartmentErrors.NotFound);
        }

        var duration = DateRange.Create(command.StartDate, command.EndDate);

        bool isOverlapping = await bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken);

        if (isOverlapping)
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }

        try
        {
            Booking booking = Booking.Reserve(
                apartment,
                user.Id,
                duration,
                dateTimeProvider.UtcNow,
                pricingService);

            bookingRepository.Add(booking);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(booking.Id);
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }
    }
}

internal sealed class ReserveBookingCommandValidator : AbstractValidator<ReserveBookingCommand>
{
    public ReserveBookingCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty();

        RuleFor(command => command.ApartmentId).NotEmpty();

        RuleFor(command => command.StartDate).LessThan(command => command.EndDate);
    }
}