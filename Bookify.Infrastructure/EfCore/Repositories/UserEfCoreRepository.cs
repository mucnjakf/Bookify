using Bookify.Domain.Users;
using Bookify.Infrastructure.EfCore.Repositories.Abstractions;

namespace Bookify.Infrastructure.EfCore.Repositories;

internal sealed class UserEfCoreRepository(ApplicationDbContext dbContext)
    : EfCoreRepository<User>(dbContext), IUserRepository;