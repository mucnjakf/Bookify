using Bookify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.EfCore;

internal sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InfrastructureModule).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}