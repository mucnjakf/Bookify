using Bookify.Domain.Users;

namespace Bookify.Application.Abstractions.Notifications;

internal interface IEmailService
{
    Task SendAsync(Email recipient, string subject, string body);
}