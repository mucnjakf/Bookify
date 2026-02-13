using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Users;

namespace Bookify.Application.Auth.LoginUser;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<TokenDto>;

internal sealed class LoginUserCommandHandler(IJwtService jwtService) : ICommandHandler<LoginUserCommand, TokenDto>
{
    public async Task<Result<TokenDto>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        Result<string> result = await jwtService.GetTokenAsync(command.Email, command.Password, cancellationToken);

        return result.IsFailure
            ? Result.Failure<TokenDto>(UserErrors.InvalidCredentials)
            : Result.Success(new TokenDto(result.Value));
    }
}