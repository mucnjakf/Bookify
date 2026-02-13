using Bookify.Domain.Users;
using Bookify.Infrastructure.EfCore.Repositories.Abstractions;

namespace Bookify.Infrastructure.EfCore.Repositories;

internal sealed class UserEfCoreRepository(ApplicationDbContext dbContext)
    : EfCoreRepository<User>(dbContext), IUserRepository
{
    public override void Add(User user)
    {
        foreach (Role role in user.Roles)
        {
            DbContext.Attach(role);
        }

        DbContext.Add(user);
    }
}