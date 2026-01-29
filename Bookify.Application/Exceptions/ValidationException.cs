namespace Bookify.Application.Exceptions;

internal sealed class ValidationException(IEnumerable<ValidationError> errors) : Exception
{
    public IEnumerable<ValidationError> Errors { get; } = errors;
}

internal sealed record ValidationError(string PropertyName, string ErrorMessage);