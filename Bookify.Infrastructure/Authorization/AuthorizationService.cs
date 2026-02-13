using Bookify.Domain.Users;
using Bookify.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class AuthorizationService(ApplicationDbContext dbContext)
{
    public async Task<UserRolesDto> GetRolesForUserAsync(string identityId) =>
        await dbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .Select(user => new UserRolesDto(user.Id, user.Roles.ToList()))
            .FirstAsync();
}