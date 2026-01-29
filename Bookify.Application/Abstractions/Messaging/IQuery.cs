using Bookify.Domain.Abstractions;
using MediatR;

namespace Bookify.Application.Abstractions.Messaging;

internal interface IQuery<TResponse> : IRequest<Result<TResponse>>;