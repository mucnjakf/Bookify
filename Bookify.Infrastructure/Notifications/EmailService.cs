using Bookify.Application.Abstractions.Notifications;
using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Notifications;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(Email recipient, string subject, string body) => Task.CompletedTask;
}