namespace Bookify.Domain.Users;

internal interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(User user);
}