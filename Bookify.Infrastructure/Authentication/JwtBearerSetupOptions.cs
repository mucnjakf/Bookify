using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Authentication;

internal sealed class JwtBearerSetupOptions(IOptions<AuthenticationOptions> authenticationOptions)
    : IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(JwtBearerOptions options)
    {
        options.Audience = authenticationOptions.Value.Audience;
        options.MetadataAddress = authenticationOptions.Value.MetadataUrl;
        options.RequireHttpsMetadata = authenticationOptions.Value.RequireHttpsMetadata;
        options.TokenValidationParameters.ValidIssuer = authenticationOptions.Value.Issuer;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}