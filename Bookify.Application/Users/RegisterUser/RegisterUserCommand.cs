using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Users;
using FluentValidation;

namespace Bookify.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password)
    : ICommand<Guid>;

internal sealed class RegisterUserCommandHandler(
    IAuthenticationService authenticationService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = User.Create(
            new FirstName(command.FirstName),
            new LastName(command.LastName),
            new Email(command.Email));

        string identityId = await authenticationService.RegisterAsync(user, command.Password, cancellationToken);

        user.SetIdentityId(identityId);

        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(user => user.FirstName).NotEmpty();

        RuleFor(user => user.LastName).NotEmpty();

        RuleFor(user => user.Email).EmailAddress();

        RuleFor(user => user.Password).NotEmpty();
    }
}