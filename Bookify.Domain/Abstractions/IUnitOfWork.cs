namespace Bookify.Domain.Abstractions;

internal interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}