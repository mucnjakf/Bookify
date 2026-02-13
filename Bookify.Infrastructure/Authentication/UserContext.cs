using Bookify.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Bookify.Infrastructure.Authentication;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                          throw new ApplicationException("Failed to get current user ID");

    public string IdentityId => httpContextAccessor.HttpContext?.User.GetUserIdentityId() ??
                                throw new ApplicationException("Failed to get current user identity ID");
}