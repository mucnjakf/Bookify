using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Bookify.Application.Abstractions.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = request.GetType().Name;

        try
        {
            logger.LogInformation("Executing request {Request}", requestName);

            TResponse result = await next(cancellationToken);

            if (result.IsSuccess)
            {
                logger.LogInformation("Request {Request} processed successfully", requestName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Request {Request} processed with error", requestName);
                }
            }

            return result;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Request {Request} processing failed", requestName);
            throw;
        }
    }
}