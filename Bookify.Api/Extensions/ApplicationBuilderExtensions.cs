using Bookify.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Api.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        applicationDbContext.Database.Migrate();
    }
}