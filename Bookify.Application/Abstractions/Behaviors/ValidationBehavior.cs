using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using FluentValidation;
using MediatR;
using ValidationException = Bookify.Application.Exceptions.ValidationException;

namespace Bookify.Application.Abstractions.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        List<ValidationError> validationErrors = validators
            .Select(validator => validator.Validate(context))
            .Where(validationResult => validationResult.Errors.Count != 0)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new ValidationError(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage))
            .ToList();

        if (validationErrors.Count != 0)
        {
            throw new ValidationException(validationErrors);
        }

        return await next(cancellationToken);
    }
}