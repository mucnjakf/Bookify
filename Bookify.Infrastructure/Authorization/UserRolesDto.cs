using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Authorization;

public sealed record UserRolesDto(Guid UserId, List<Role> Roles);