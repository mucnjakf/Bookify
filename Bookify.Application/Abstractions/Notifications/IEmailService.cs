using Bookify.Domain.Users;

namespace Bookify.Application.Abstractions.Notifications;

public interface IEmailService
{
    Task SendAsync(Email recipient, string subject, string body);
}