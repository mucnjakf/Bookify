using Bookify.Domain.Apartments;
using Bookify.Infrastructure.EfCore.Repositories.Abstractions;

namespace Bookify.Infrastructure.EfCore.Repositories;

internal sealed class ApartmentEfCoreRepository(ApplicationDbContext dbContext)
    : EfCoreRepository<Apartment>(dbContext), IApartmentRepository;