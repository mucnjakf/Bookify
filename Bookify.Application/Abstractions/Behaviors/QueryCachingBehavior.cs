using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstractions.Behaviors;

internal sealed class QueryCachingBehavior<TRequest, TResponse>(
    ICacheService cacheService,
    ILogger<QueryCachingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest query,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var cachedResult = await cacheService.GetAsync<TResponse>(query.CacheKey, cancellationToken);

        string queryName = typeof(TRequest).Name;

        if (cachedResult is not null)
        {
            logger.LogInformation("Cache hit for {QueryName}", queryName);

            return cachedResult;
        }

        logger.LogInformation("Cache miss for {QueryName}", queryName);

        TResponse result = await next(cancellationToken);

        if (result.IsSuccess)
        {
            await cacheService.SetAsync(query.CacheKey, result, query.Expiration, cancellationToken);
        }

        return result;
    }
}