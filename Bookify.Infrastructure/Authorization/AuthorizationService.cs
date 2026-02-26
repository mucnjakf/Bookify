using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Users;
using Bookify.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class AuthorizationService(ApplicationDbContext dbContext, ICacheService cacheService)
{
    public async Task<UserRolesDto> GetRolesForUserAsync(string identityId)
    {
        var cacheKey = $"auth:roles-{identityId}";

        var cachedUserRoles = await cacheService.GetAsync<UserRolesDto>(cacheKey);

        if (cachedUserRoles is not null)
        {
            return cachedUserRoles;
        }

        UserRolesDto userRoles = await dbContext
            .Set<User>()
            .Where(user => user.IdentityId == identityId)
            .Select(user => new UserRolesDto(user.Id, user.Roles.ToList()))
            .FirstAsync();

        await cacheService.SetAsync(cacheKey, userRoles);

        return userRoles;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
    {
        var cacheKey = $"auth-permissions-{identityId}";

        var cachedPermissions = await cacheService.GetAsync<HashSet<string>>(cacheKey);

        if (cachedPermissions is not null)
        {
            return cachedPermissions;
        }

        ICollection<Permission> permissions = await dbContext
            .Set<User>()
            .Where(user => user.IdentityId == identityId)
            .SelectMany(user => user.Roles.Select(role => role.Permissions))
            .FirstAsync();

        HashSet<string> permissionsHashSet = permissions.Select(permission => permission.Name).ToHashSet();

        await cacheService.SetAsync(cacheKey, permissionsHashSet);

        return permissionsHashSet;
    }
}