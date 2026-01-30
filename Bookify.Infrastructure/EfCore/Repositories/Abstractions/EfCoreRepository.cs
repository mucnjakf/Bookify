using Bookify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.EfCore.Repositories.Abstractions;

internal abstract class EfCoreRepository<T>(ApplicationDbContext dbContext)
    where T : Entity
{
    protected readonly ApplicationDbContext DbContext = dbContext;

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await DbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

    public void Add(T entity) => DbContext.Add(entity);
}