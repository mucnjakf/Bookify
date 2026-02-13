using Bookify.Domain.Abstractions;

namespace Bookify.Application.Abstractions.Authentication;

public interface IJwtService
{
    Task<Result<string>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
}