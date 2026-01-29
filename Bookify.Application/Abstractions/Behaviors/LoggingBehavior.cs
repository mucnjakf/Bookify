using Bookify.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstractions.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string commandName = request.GetType().Name;

        try
        {
            logger.LogInformation("Executing command {Command}", commandName);

            TResponse result = await next(cancellationToken);

            logger.LogInformation("Command {Command} processed successfully", commandName);

            return result;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Command {Command} processing failed", commandName);
            throw;
        }
    }
}