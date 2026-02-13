using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Bookify.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        string? userId = claimsPrincipal?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(userId, out Guid parsedUserId)
            ? parsedUserId
            : throw new ApplicationException("Unable to get user ID from claims principal");
    }

    public static string GetUserIdentityId(this ClaimsPrincipal? claimsPrincipal) =>
        claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier) ??
        throw new ApplicationException("Unable to get user identity ID from claims principal");
}